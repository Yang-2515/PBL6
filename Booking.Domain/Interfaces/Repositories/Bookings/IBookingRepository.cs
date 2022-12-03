using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Domain.Interfaces.Repositories.Bookings
{
    public interface IBookingRepository : IGenericRepository<BookingEntity>
    {
        Task<BookingEntity> GetAsync(int id);
        IQueryable<BookingEntity> GetBookingOutOfDay();
        Task<IEnumerable<BookingEntity>> GetBookingDueForPaymentAsync();
        Task<IEnumerable<BookingEntity>> GetBookingMustExtendDueAsync();
        Task<bool> IsHiredAsync(int roomId);
    }
}
