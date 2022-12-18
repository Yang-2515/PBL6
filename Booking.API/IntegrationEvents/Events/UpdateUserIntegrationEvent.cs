using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class UpdateUserIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public UpdateUserIntegrationEvent()
        {

        }
        public UpdateUserIntegrationEvent(string userId, string name, string businessId, string? avatar)
        {
            UserId = userId;
            Name = name;
            Avatar = avatar;
        }
    }
}
