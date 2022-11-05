namespace Booking.API.ViewModel.Bookings.Request
{
    public class AddBookingRequest
    {
        public int RoomId { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime FinishDay { get; set; }
        public List<BookingUtilityRequest> Utilities { get; set; }
    }
    public class BookingUtilityRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
