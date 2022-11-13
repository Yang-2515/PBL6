using Booking.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Booking.Domain.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; private set; }
        public virtual ICollection<District> Districts { get; private set; }
    }
}
