using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class DeleteUserIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public DeleteUserIntegrationEvent()
        {

        }
        public DeleteUserIntegrationEvent(string userId)
        {
            UserId = userId;
        }
    }
}
