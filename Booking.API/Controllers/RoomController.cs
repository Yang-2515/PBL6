using Booking.API.Services;
using Booking.API.ViewModel.Rooms.Request;
using Booking.API.ViewModel.Rooms.Response;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("/locations/{locationId:int}/rooms/all")]
        public async Task<List<RoomBasicInfoResponse>> GetAll([FromRoute] int locationId, [FromBody] RoomBasicInfoRequest request)
        {
            return await _roomService.GetByFilter(locationId, request);
        }

        [HttpPost("/locations/{locationId:int}/rooms")]
        public async Task<bool> CreateRoom([FromRoute] int locationId, [FromBody] CreateRoomRequest request)
        {
            return await _roomService.CreateAsync(locationId, request);
        }

    }
}
