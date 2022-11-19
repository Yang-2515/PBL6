using Booking.Domain.Interfaces.Repositories.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Booking
    {
        public Booking(int roomId, DateTime startDay, DateTime finishDay, string userId, string userName, string businessId)        {
            RoomId = roomId;
            StartDay = startDay;
            FinishDay = finishDay;
            UserId = userId;
            UserName = userName;
            BusinessId = businessId;
            Status = BookingStatus.Pending;
            BookingUtilities = new List<BookingUtility>();
        }

        public void AddUtility(int utilityId, string name, int price)
        {
            BookingUtilities.Add(new BookingUtility(Id, utilityId, name, price));
        }

        public void Update(DateTime startDate, DateTime finishDate, IBookingUtilityRepository _bookingUtilityRepository)
        {
            StartDay = startDate;
            FinishDay = finishDate;
            _bookingUtilityRepository.RemoveRange(BookingUtilities);
        }
        public void Delete()
        {
            IsDelete = true;
        }
    }
}
