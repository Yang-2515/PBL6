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
        public Booking(int roomId, DateTime startDay, int monthNumber, string userId, string userName, string businessId)        {
            RoomId = roomId;
            StartDay = startDay;
            MonthNumber = monthNumber;
            UserId = userId;
            UserName = userName;
            BusinessId = businessId;
            Status = BookingStatus.Pending;
            BookingUtilities = new List<BookingUtility>();
            CreateOn = DateTime.UtcNow;
        }

        public void AddUtility(int utilityId, string name, int price)
        {
            BookingUtilities.Add(new BookingUtility(Id, utilityId, name, price));
        }

        public void Update(DateTime startDate, int monthNumber, IBookingUtilityRepository _bookingUtilityRepository)
        {
            StartDay = startDate;
            MonthNumber = monthNumber;
            _bookingUtilityRepository.RemoveRange(BookingUtilities);
            UpdateOn = DateTime.UtcNow;
        }

        public void UpdateStatus(BookingStatus status)
        {
            Status = status;
        }

        public void Delete()
        {
            IsDelete = true;
        }
    }
}
