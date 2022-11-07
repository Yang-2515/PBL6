using Booking.API.ViewModel.Locations.Response;

namespace Booking.API.ViewModel.Rooms.Response
{
    public class RoomBasicInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
    }
}
