using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class NotificationBooking : InfoEntity
    {
        public string? NotiByUserId { get; set; }
        public string? NotiByUserName { get; set; }
        public string NotiToUserId { get; private set; }
        public string Message { get; private set; }
        public int BookingId { get; private set; }
        public virtual Booking Booking { get; private set; }
    }
}
