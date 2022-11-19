namespace Booking.API.ViewModel.Reviews.Request
{
    public class UpdateReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImgUrl { get; set; }
    }
}
