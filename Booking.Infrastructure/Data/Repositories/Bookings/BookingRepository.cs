using Booking.Domain;
using Booking.Domain.Interfaces.Repositories.Bookings;
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

        public IQueryable<BookingEntity> GetBookingOutOfDay()
        {
            return GetQuery(_ => !_.IsDelete 
                            && (DateTime.UtcNow - _.ApprovedOn).TotalDays > 1 
                            && _.Status == BookingStatus.Approved);
        }
    }
}
