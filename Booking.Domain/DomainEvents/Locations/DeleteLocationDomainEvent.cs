using Booking.Domain.Base;

namespace Booking.Domain.DomainEvents.Locations
{
    public class DeleteLocationDomainEvent : BaseDomainEvent
    {
        public DeleteLocationDomainEvent(int locationId)
        {
            LocationId = locationId;
        }

        public int LocationId { get; set; }
    }
}
