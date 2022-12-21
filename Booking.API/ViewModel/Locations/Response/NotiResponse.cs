namespace Booking.API.ViewModel.Locations.Response
{
    public class NotiResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public int BookingId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
