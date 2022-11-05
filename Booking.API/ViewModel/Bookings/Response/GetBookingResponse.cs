namespace Booking.API.ViewModel.Bookings.Response
{
    public class GetBookingResponse
    { 
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime FinishDay { get; set; }
        public string Status { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
    }
}
