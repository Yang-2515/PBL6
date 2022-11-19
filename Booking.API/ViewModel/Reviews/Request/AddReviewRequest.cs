namespace Booking.API.ViewModel.Reviews.Request
{
    public class AddReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public IFormFile Img { get; set; }
    }
}
