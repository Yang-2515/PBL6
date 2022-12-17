using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class NotificationBooking
    {
        public NotificationBooking(string ? notiByUserId, string? notiByUserName, string notiToUserId, string message)
        {
            NotiByUserId = notiByUserId;
            NotiByUserName = notiByUserName;
            NotiToUserId = notiToUserId;
            Message = message;
            CreateOn = DateTime.UtcNow;
        }
    }
}
