using EventBus.Events;
using System.Text.Json.Serialization;

namespace Booking.API.IntegrationEvents.Events
{
    public class UserCreatedIntergrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? BusinessId { get; set; }
        public string? Avatar { get; set; }
        [JsonConstructor]
        public UserCreatedIntergrationEvent()
        {

        }
        public UserCreatedIntergrationEvent(string id, string name, string? businessId, string? avatar)
        {
            UserId = id;
            Name = name;
            BusinessId = businessId;
            Avatar = avatar;
        }
    }
}
