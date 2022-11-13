using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class District
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; private set; }
        public int CityId { get; private set; }
        public virtual City City { get; private set; }
        public virtual ICollection<Wards> Wardses { get; private set; }
    }
}
