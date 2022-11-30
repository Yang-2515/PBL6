using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class UpdateUserIntegrationEvent : IntegrationEvent
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string? Avatar { get; private set; }
        public UpdateUserIntegrationEvent()
        {

        }
        public UpdateUserIntegrationEvent(string id, string name, string businessId, string? avatar)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
        }
    }
}
