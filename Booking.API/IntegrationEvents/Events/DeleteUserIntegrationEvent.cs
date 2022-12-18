using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class DeleteUserIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; }
        public DeleteUserIntegrationEvent()
        {

        }
        public DeleteUserIntegrationEvent(string id)
        {
            Id = id;
        }
    }
}
