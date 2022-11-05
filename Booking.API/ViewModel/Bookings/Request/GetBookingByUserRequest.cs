using Booking.API.ViewModel.Bookings.Response;
using Booking.API.ViewModel.Interfaces;
using System.Linq.Expressions;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.API.ViewModel.Bookings.Request
{
    public class GetBookingByUserRequest : IFilter<BookingEntity>, ISelection<BookingEntity, GetBookingResponse>
    {
        private int _userId { get; set; }
        public void SetId(int userId) => _userId = userId;
        public int GetId() => _userId;

        public Expression<Func<BookingEntity, bool>> GetFilter()
        {
            return _ => _.UserId == GetId() && !
        }

        public Expression<Func<BookingEntity, GetBookingResponse>> GetSelection()
        {
            throw new NotImplementedException();
        }
    }
}
