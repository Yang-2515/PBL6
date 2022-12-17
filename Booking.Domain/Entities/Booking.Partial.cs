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
            NotificationBookings = new List<NotificationBooking>();
            DuePayment = startDay;
            CreateOn = DateTime.UtcNow;
        }

        public void AddUtility(int utilityId, string name, int price)
        {
            BookingUtilities.Add(new BookingUtility(Id, utilityId, name, price));
        }

        public void AddNoti(string userId, string? username, string message, string notiToUserId)
        {
            NotificationBookings.Add(new NotificationBooking(userId
                                                            , username
                                                            , notiToUserId
                                                            , message));
        }

        public void Update(int monthNumber)
        {
            MonthNumber = monthNumber;
            UpdateOn = DateTime.UtcNow;
        }

        public void UpdateStatus(BookingStatus status)
        {
            if (status == BookingStatus.Approved)
                ApprovedOn = DateTime.UtcNow;
            Status = status;
        }

        public void Delete()
        {
            IsDelete = true;
        }

        public void UpdateDuePayment(int? number)
        {
            if (number == null)
                DuePayment = StartDay;
            else
            {
                var datetime = DuePayment.Value.AddMonths(number.Value);
                DuePayment = datetime;
            }
        }
    }
}
