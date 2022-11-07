using System.ComponentModel.DataAnnotations;

namespace Booking.API.ViewModel.Rooms.Request
{
    public class CreateRoomRequest
    {
        [Required]
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
    }
}
