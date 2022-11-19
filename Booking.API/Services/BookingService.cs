using Booking.API.ViewModel.Bookings.Request;
using Booking.API.ViewModel.Bookings.Response;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Booking.Domain.Entities.Booking;
using ErrorMessages = Booking.Domain.Entities.MessageResource;

namespace Booking.API.Services
{
    public class BookingService : ServiceBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingUtilityRepository _bookingUtilityRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IBookingRepository bookingRepository
            , IBookingUtilityRepository bookingUtilityRepository
            , IUnitOfWork unitOfWork
            , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _bookingUtilityRepository = bookingUtilityRepository;
        }
        public async Task<List<GetBookingResponse>> GetBookingByUserAsync()
        {
            var request = new GetBookingRequest();
            return await _bookingRepository.GetQuery(request.GetFilterByUser(GetCurrentUserId().Id))
                        .Select(request.GetSelection())
                        .ToListAsync();
        }

        public async Task<List<GetBookingResponse>> GetBookingByBusinessAsync()
        {
            var request = new GetBookingRequest();
            return await _bookingRepository.GetQuery(request.GetFilterByBusiness(1))
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

            booking.Update(request.StartDay, request.FinishDay, _bookingUtilityRepository);

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
            var booking = new BookingEntity(request.RoomId
                    , request.StartDay
                    , request.FinishDay
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
    }
}
