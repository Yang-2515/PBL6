using Booking.Domain;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Infrastructure.Data.Repositories.Bookings
{
    public class BookingRepository : GenericRepository<BookingEntity>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<BookingEntity> GetAsync(int id)
        {
            return await GetAsync(_ => _.Id == id && !_.IsDelete);
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingDueForPaymentAsync()
        {
            var bookings = await GetQuery(_ => !_.IsDelete
                            && _.Status == BookingStatus.Success
                            && _.DuePayment.HasValue).ToListAsync();
            return bookings.Where(_ => (_.DuePayment.Value - DateTime.UtcNow).TotalDays < 1
                                        && _.DuePayment.Value.Month != _.Room.AvailableDay.Value.Month);
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingMustExtendDueAsync()
        {
            var bookings = await GetQuery(_ => !_.IsDelete
                            && _.Status == BookingStatus.Success
                            && _.Room.AvailableDay.HasValue).ToListAsync();
            return bookings.Where(_ => (_.Room.AvailableDay.Value - DateTime.UtcNow).TotalDays < 1);
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingOutOfDay()
        {
            var bookings = await GetQuery(_ => !_.IsDelete
                            && _.Status == BookingStatus.Approved
                            && _.ApprovedOn.HasValue).ToListAsync();
            return bookings.Where(_ => (DateTime.UtcNow - _.ApprovedOn.Value).TotalDays > 1);
        }

        public async Task<IEnumerable<BookingEntity>> GetBookingOverDuePaymentAsync()
        {
            return await GetQuery(_ => !_.IsDelete
                            && _.DuePayment < DateTime.UtcNow
                            && _.DuePayment.Value.AddMonths(1) < _.Room.AvailableDay
                            && _.Status == BookingStatus.Success)
                        .ToListAsync();
        }

        public async Task<bool> IsHiredAsync(int roomId)
        {
            var bookings = await GetQuery(_ => !_.IsDelete 
                            && _.RoomId == roomId
                            && _.Room.AvailableDay.HasValue).ToListAsync();
            return bookings.Where(_ => _.Room.AvailableDay >= DateTime.UtcNow
                            && _.Status == BookingStatus.Success).Any();
        }
    }
}
