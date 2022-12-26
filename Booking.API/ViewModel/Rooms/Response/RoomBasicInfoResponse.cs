using Booking.API.ViewModel.Locations.Response;
using Newtonsoft.Json;

namespace Booking.API.ViewModel.Rooms.Response
{
    public class RoomBasicInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public string? AvailableDay { get; set; }
        public string? ImgId { get; set; }
        public string? ImgUrl { get; set; }
        public int? LocationId { get; set; }
        public double Rating { get; set; }
    }
}
