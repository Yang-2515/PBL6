﻿using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class UserCreatedIntergrationEvent : IntegrationEvent
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string BusinessId { get; private set; }
        public string? Avatar { get; private set; }
        public UserCreatedIntergrationEvent()
        {

        }
        public UserCreatedIntergrationEvent(string id, string name, string businessId, string? avatar)
        {
            Id = id;
            Name = name;
            BusinessId = businessId;
            Avatar = avatar;
        }
    }
}
