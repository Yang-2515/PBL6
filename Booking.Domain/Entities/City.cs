using Booking.Domain.Base;

namespace Booking.Domain.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; private set; }
        public virtual ICollection<District> Districts { get; private set; }
    }
}
