using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class Room : Entity
    {
        public string Name { get; private set; }
        public int LocationId { get; private set; }
        public int BusinessId { get; private set; }
        public int Capacity { get; private set; }
        public int Price { get; private set; }
        public bool IsActive { get; private set; }
        public virtual Location Location { get; private set; }
    }
}
