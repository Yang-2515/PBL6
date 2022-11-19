namespace Booking.API.ViewModel.Reviews.Request
{
    public class AddReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImgUrl { get; set; }
    }
}
