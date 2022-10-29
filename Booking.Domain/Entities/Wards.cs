using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class Wards : BaseEntity
    {
        public string Name { get; private set; }
        public int DistrictId { get; private set; }
        public virtual District District { get; private set; }
    }
}
