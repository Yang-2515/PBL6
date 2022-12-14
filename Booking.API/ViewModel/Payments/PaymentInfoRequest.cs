namespace Booking.API.ViewModel.Payments
{
    public class PaymentInfoRequest
    {
        public int BookingId { get; set; }
        public int Amount { get; set; }
        public string OrderDesc { get; set; }
        public LanguageType Language { get; set; }
        public BankType Bank { get; set; }
        public OrderCategoryType OrderCategory { get; set; }

        public string BillingMobile { get; set; }
        public string BillingEmail { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingCountry { get; set; }

        public string ShippingMobile { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingCustomer { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCompany { get; set; }
        public string ShippingTaxCode { get; set; }
        public BillType ShippingBillType { get; set; }
    }
}
