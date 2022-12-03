using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Bookings
{
    public class NotificationBookingRepository : GenericRepository<NotificationBooking>, INotificationBookingRepository
    {
        public NotificationBookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
