using Booking.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class Wards
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; private set; }
        public int DistrictId { get; private set; }
        public virtual District District { get; private set; }
    }
}
