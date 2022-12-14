using Booking.API.Extensions;
using Booking.API.IntegrationEvents.Events;
using Booking.API.ViewModel.Bookings.Request;
using Booking.API.ViewModel.Bookings.Response;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Rooms;
using Booking.Domain.Interfaces.Repositories.Users;
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Booking.Domain.Entities.Booking;
using ErrorMessages = Booking.Domain.Entities.MessageResource;

namespace Booking.API.Services
{
    public class BookingService : ServiceBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingUtilityRepository _bookingUtilityRepository;
        private readonly IRoomRepository _roomRepo;
        private readonly INotificationBookingRepository _notificationBookingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IBookingRepository bookingRepository
            , IBookingUtilityRepository bookingUtilityRepository
            , IUnitOfWork unitOfWork
            , IRoomRepository roomRepo
            , INotificationBookingRepository notificationBookingRepo
            , IUserRepository userRepo
            , IEventBus eventBus
            , ILogger<BookingService> logger
            , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _bookingUtilityRepository = bookingUtilityRepository;
            _roomRepo = roomRepo;
            _notificationBookingRepo = notificationBookingRepo;
            _userRepo = userRepo;
            _eventBus = eventBus;
            _logger = logger;
        }
        public async Task<List<GetBookingResponse>> GetBookingByUserAsync(BookingStatus status)
        {
            var request = new GetBookingRequest();
            return await _bookingRepository.GetQuery(request.GetFilterByUser(GetCurrentUserId().Id, status))
                                            .OrderByDescending(_ => _.CreateOn)
                                            .Select(request.GetSelection())
                                            .ToListAsync();
        }

        public async Task<List<NotiResponse>> GetNotiBookingByUserAsync()
        {
            return await _notificationBookingRepo.GetQuery(_ => _.NotiToUserId == GetCurrentUserId().Id)
                                                .Select(_ => new NotiResponse
                                                {
                                                    UserId = _.NotiByUserId,
                                                    Username = _.NotiByUserName,
                                                    Message = _.Message + " " + _.Booking.Room.Name + " tại " + _.Booking.Room.Location.Name,
                                                    BookingId = _.BookingId,
                                                    CreateOn = _.CreateOn.Value
                                                })
                                                .OrderByDescending(_ => _.CreateOn)
                                                .ToListAsync();
        }

        public async Task<int> ApproveAsync(int id)
        {
            var booking = await GetBookingAsync(id);
            var isExistsApprovedBooking = await CheckExistsApprovedBooking(booking.RoomId);
            if (isExistsApprovedBooking)
                throw new BadRequestException(ErrorMessages.IsExistsApprovedBooking);
            
            booking.UpdateStatus(Domain.BookingStatus.Approved);
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã đồng ý yêu cầu thuê phòng", booking.UserId);

            await _unitOfWork.SaveChangeAsync();
            return booking.Id;
        }

        public async Task<int> RejectAsync(int id)
        {
            var booking = await GetBookingAsync(id);
            booking.UpdateStatus(Domain.BookingStatus.Reject);
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã hủy bỏ yêu cầu thuê phòng", booking.UserId);
            await _unitOfWork.SaveChangeAsync();
            
            return booking.Id;
        }

        public async Task<bool> CheckExistsApprovedBooking(int roomId)
        {
            return await _bookingRepository.AnyAsync(_ => _.RoomId == roomId 
                                                        && !_.IsDelete
                                                        && _.Status == Domain.BookingStatus.Success && _.Room.AvailableDay >= DateTime.UtcNow);
        }

        public async Task<List<GetBookingResponse>> GetBookingByBusinessAsync()
        {
            var request = new GetBookingRequest();
            return await _bookingRepository.GetQuery(request.GetFilterByBusiness(GetCurrentUserId().BusinessId))
                        .OrderBy(_ => _.CreateOn)
                        .Select(request.GetSelection())
                        .ToListAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var booking = await GetBookingAsync(id);
            booking.Delete();
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã xóa yêu cầu thuê phòng", booking.Room.Location.OwnerId);
            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<int> UpdateAsync(int id, UpdateBookingRequest request)
        {
            var booking = await GetBookingAsync(id);

            booking.Update(request.MonthNumber);

            await _bookingRepository.UpdateAsync(booking);
            var room = await ValidateOnGetRoom(booking.RoomId);
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã gia hạn yêu cầu thuê phòng", booking.Room.Location.OwnerId);
            room.HandleBookingSuccess(request.MonthNumber);

            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<int> AddAsync(AddBookingRequest request)
        {
            var room = await ValidateOnGetRoom(request.RoomId);
            if (request.StartDay < room.AvailableDay)
                throw new BadRequestException(ErrorMessages.IsNotValidStartDay);
            var booking = new BookingEntity(request.RoomId
                    , request.StartDay
                    , request.MonthNumber
                    , GetCurrentUserId().Id
                    , GetCurrentUserId().Name
                    , GetCurrentUserId().BusinessId);
            if (request.Utilities.Any())
            {
                foreach(var item in request.Utilities)
                {
                    booking.AddUtility(item.Id, item.Name, item.Price);
                }
            }
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã tạo yêu cầu thuê phòng", room.Location.OwnerId);
            await SendMessageToFirebase(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã tạo yêu cầu thuê phòng", room.Location.OwnerId);
            await _bookingRepository.InsertAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<BookingEntity> GetBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundBooking);

            return booking;
        }

        public async Task<Room> ValidateOnGetRoom(int roomId)
        {
            var room = await _roomRepo.GetAsync(roomId);
            if (room == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundRoom);
            return room;
        }

        public async Task FirstPaymentSuccess(int bookingId)
        {
            var booking = await GetBookingAsync(bookingId);
            booking.UpdateStatus(Domain.BookingStatus.Success);
            booking.UpdateDuePayment(1);
            var room = await ValidateOnGetRoom(booking.RoomId);
            var bookings = await _bookingRepository.GetQuery(_ => _.RoomId == booking.RoomId && _.Status == Domain.BookingStatus.Pending && !_.IsDelete).ToListAsync();
            foreach (var item in bookings)
            {
                item.UpdateStatus(Domain.BookingStatus.Reject);
                var username = await _userRepo.GetQuery(_ => _.Id == room.Location.OwnerId).Select(x => x.Name).FirstOrDefaultAsync();
                item.AddNoti(room.Location.OwnerId, username, "đã hủy bỏ yêu cầu thuê phòng", booking.UserId);
            }
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã thanh toán thành công phòng", room.Location.OwnerId);
            room.HandleBookingSuccess(booking.MonthNumber);
            var paymentSuccessEvent = new PaymentSuccessIntegrationEvent(GetCurrentUserId().Id, room.BusinessId);
            
            try
            {
                _eventBus.Publish(paymentSuccessEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: PaymentSuccess");

                throw;
            }
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task PaymentSuccess(int bookingId)
        {
            var booking = await GetBookingAsync(bookingId);
            booking.UpdateDuePayment(1);
            var room = await ValidateOnGetRoom(booking.RoomId);
            booking.AddNoti(GetCurrentUserId().Id, GetCurrentUserId().Name, "đã thanh toán thành công phòng", room.Location.OwnerId);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
