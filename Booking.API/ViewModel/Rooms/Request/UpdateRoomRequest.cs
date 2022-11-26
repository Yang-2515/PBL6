using System.ComponentModel.DataAnnotations;

namespace Booking.API.ViewModel.Rooms.Request
{
    public class UpdateRoomRequest
    {
        [Required]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public string? ImgId { get; set; }
    }
}
