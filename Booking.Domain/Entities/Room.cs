using Booking.Domain.Base;

namespace Booking.Domain.Entities
{
    public partial class Room : Entity
    {
        public string Name { get; private set; }
        public int LocationId { get; private set; }
        public string BusinessId { get; private set; }
        public int Capacity { get; private set; }
        public int Price { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsBooked { get; private set; }
        public DateTime AvailableDay { get; private set; }
        public virtual Location Location { get; private set; }
        public virtual ICollection<Review> Reviews { get; private set; }
    }
}
