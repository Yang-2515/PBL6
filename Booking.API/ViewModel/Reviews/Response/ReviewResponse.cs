using Newtonsoft.Json;

namespace Booking.API.ViewModel.Reviews.Response
{
    public class ReviewResponse
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        [JsonIgnore]
        public string ImgId { get; set; }
        public string ImgUrl { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
    }
}
