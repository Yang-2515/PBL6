using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Booking : Entity
    {
        public int BusinessId { get; private set; }
        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public DateTime StartDay { get; private set; }
        public DateTime FinishDay { get; private set; }
        public BookingStatus Status { get; private set; }
        public virtual Room Room { get; private set; }
        public virtual ICollection<BookingUtility> BookingUtilities { get; private set; }   
    }
}
