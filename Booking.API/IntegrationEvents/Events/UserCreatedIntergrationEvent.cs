using EventBus.Events;
using System.Text.Json.Serialization;

namespace Booking.API.IntegrationEvents.Events
{
    public class UserCreatedIntergrationEvent : IntegrationEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserCreatedIntergrationEvent()
        {

        }
        public UserCreatedIntergrationEvent(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
