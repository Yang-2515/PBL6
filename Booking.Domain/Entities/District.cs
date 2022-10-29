using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; private set; }
        public int CityId { get; private set; }
        public virtual City City { get; private set; }
        public virtual ICollection<Wards> Wardses { get; private set; }
    }
}
