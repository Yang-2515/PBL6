using Booking.Domain.Base;

namespace Booking.Domain.Entities
{
    public partial class Payment : Entity
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public string UserId { get; set; }
        public bool? Status { get; set; }
        public int Amount { get; set; }
        public string? TranCode { get; set; }
        public string? PaymentCode { get; set; }
        public string OrderDesc { get; set; }
    }
}
