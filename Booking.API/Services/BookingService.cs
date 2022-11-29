using Booking.API.IntegrationEvents.Events;
using Booking.API.ViewModel.Bookings.Request;
using Booking.API.ViewModel.Bookings.Response;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Rooms;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IBookingRepository bookingRepository
            , IBookingUtilityRepository bookingUtilityRepository
            , IUnitOfWork unitOfWork
            , IRoomRepository roomRepo
            , IEventBus eventBus
            , ILogger<BookingService> logger
            , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _bookingUtilityRepository = bookingUtilityRepository;
            _roomRepo = roomRepo;
            _eventBus = eventBus;
            _logger = logger;
        }
        public async Task<List<GetBookingResponse>> GetBookingByUserAsync()
        {
            var request = new GetBookingRequest();
            return await _bookingRepository.GetQuery(request.GetFilterByUser(GetCurrentUserId().Id))
                                            .OrderByDescending(_ => _.CreateOn)
                                            .Select(request.GetSelection())
                                            .ToListAsync();
        }

        public async Task<int> ApproveAsync(int id)
        {
            var booking = await GetBookingAsync(id);
            var isExistsApprovedBooking = await CheckExistsApprovedBooking(booking.RoomId);
            if (isExistsApprovedBooking)
                throw new BadHttpRequestException(ErrorMessages.IsExistsApprovedBooking);
            
            booking.UpdateStatus(Domain.BookingStatus.Approved);
            //push noti
            return booking.Id;
        }

        public async Task<int> RejectAsync(int id)
        {
            var booking = await GetBookingAsync(id);
            booking.UpdateStatus(Domain.BookingStatus.Reject);
            return booking.Id;
        }

        public async Task<bool> CheckExistsApprovedBooking(int roomId)
        {
            return await _bookingRepository.AnyAsync(_ => _.RoomId == roomId && _.Status == Domain.BookingStatus.Approved && !_.IsDelete);
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
            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<int> UpdateAsync(int id, UpdateBookingRequest request)
        {
            var booking = await GetBookingAsync(id);

            booking.Update(request.StartDay, request.MonthNumber, _bookingUtilityRepository);

            if (request.Utilities.Any())
            {
                foreach(var item in request.Utilities)
                {
                    booking.AddUtility(item.Id, item.Name, item.Price);
                }
            }
            await _bookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<int> AddAsync(AddBookingRequest request)
        {
            var room = await ValidateOnGetRoom(request.RoomId);
            if (request.StartDay < room.AvailableDay)
                throw new Exception(ErrorMessages.IsNotValidStartDay);
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

            await _bookingRepository.InsertAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return booking.Id;
        }

        public async Task<BookingEntity> GetBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundBooking);

            return booking;
        }

        public async Task<Room> ValidateOnGetRoom(int roomId)
        {
            var room = await _roomRepo.GetAsync(roomId);
            if (room == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundRoom);
            return room;
        }

        public async Task PaymentSuccess(int roomId, int bookingId)
        {
            var booking = await GetBookingAsync(bookingId);
            var room = await ValidateOnGetRoom(roomId);
            var bookings = await _bookingRepository.GetQuery(_ => _.RoomId == roomId && _.Status == Domain.BookingStatus.Pending && !_.IsDelete).ToListAsync();
            foreach (var item in bookings)
            {
                item.UpdateStatus(Domain.BookingStatus.Reject);   
            }
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
        }
    }
}
