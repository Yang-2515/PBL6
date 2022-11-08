using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Review : Entity
    {
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public string ImgUrl { get; private set; }
        public int UserId { get; private set; }
        public int RoomId { get; private set; }
        public virtual Room Room { get; private set; }
    }
}
