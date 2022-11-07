namespace Booking.API.ViewModel.Reviews.Response
{
    public class ReviewResponse
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImgUrl { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}
