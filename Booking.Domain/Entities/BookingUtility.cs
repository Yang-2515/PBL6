using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class BookingUtility : BaseEntity
    {
        public int? BookingId { get; private set; }
        public int? UtilityId { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }
        public virtual Booking? Booking { get; private set; }
        public virtual Utility? Utility { get; private set; }
    }
}
