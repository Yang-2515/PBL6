namespace Booking.API.ViewModel.Bookings.Request
{
    public class UpdateBookingRequest
    {
        public DateTime StartDay { get; set; }
        public DateTime FinishDay { get; set; }
        public List<BookingUtilityRequest> Utilities { get; set; }
    }
}
