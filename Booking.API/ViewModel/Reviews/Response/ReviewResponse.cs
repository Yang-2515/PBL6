using Newtonsoft.Json;

namespace Booking.API.ViewModel.Reviews.Response
{
    public class ReviewResponse
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string? ImgId { get; set; }
        public string? ImgUrl { get; set; }
        public string UserId { get; set; }
        public string AvatarId { get; set; }
        public string AvatarUrl { get; set; }
        public int RoomId { get; set; }
    }
}
