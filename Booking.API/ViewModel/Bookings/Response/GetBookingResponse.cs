using Booking.API.ViewModel.Locations.Response;

namespace Booking.API.ViewModel.Bookings.Response
{
    public class GetBookingResponse
    { 
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDay { get; set; }
        public int MonthNumber { get; set; }
        public string Status { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string? ImgUrl { get; set; }
        public int? OverDueDay { get; set; }
        public List<UtilityResponse> Utilities { get; set; }
    }
}
