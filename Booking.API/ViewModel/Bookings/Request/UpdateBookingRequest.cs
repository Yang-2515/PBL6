namespace Booking.API.ViewModel.Bookings.Request
{
    public class UpdateBookingRequest
    {
        public DateTime StartDay { get; set; }
        public int MonthNumber { get; set; }
        public List<BookingUtilityRequest> Utilities { get; set; }
    }
}
