using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class Booking : Entity
    {
        public int BusinessId { get; private set; }
        public int RoomId { get; private set; }
        public int UserId { get; private set; }
        public virtual Room Room { get; private set; }
        public virtual ICollection<BookingUtility> BookingUtilities { get; private set; }   
    }
}
