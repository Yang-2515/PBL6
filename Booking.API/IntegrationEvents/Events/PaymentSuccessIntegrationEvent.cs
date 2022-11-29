using EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class PaymentSuccessIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string BusinessId { get; set; }
        public PaymentSuccessIntegrationEvent()
        {

        }
        public PaymentSuccessIntegrationEvent(string userId, string businessId)
        {
            UserId = userId;
            BusinessId = businessId;
        }
    }
}
