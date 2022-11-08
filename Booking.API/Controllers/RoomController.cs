using Booking.API.Services;
using Booking.API.ViewModel.Reviews.Request;
using Booking.API.ViewModel.Reviews.Response;
using Booking.API.ViewModel.Rooms.Request;
using Booking.API.ViewModel.Rooms.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking")]
    [Authorize]
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
        public async Task<bool> CreateRoom([FromRoute] int locationId, [FromBody] AddRoomRequest request)
        {
            return await _roomService.CreateAsync(locationId, request);
        }

        #region Review

        [HttpGet("/locations/{locationId:int}/rooms/{roomId}/reviews")]
        public async Task<List<ReviewResponse>> GetReview([FromRoute] int locationId, [FromRoute] int roomId)
        {
            return await _roomService.GetAllReview(locationId, roomId);
        }

        [HttpPost("/locations/{locationId:int}/rooms/{roomId}/reviews")]
        public async Task<bool> AddReview([FromRoute] int locationId, [FromRoute] int roomId, [FromBody] AddReviewRequest request)
        {
            return await _roomService.AddReview(locationId, roomId, request);
        }

        [HttpDelete("/locations/{locationId:int}/rooms/{roomId}/reviews/{reviewId}")]
        public async Task<bool> DeleteReview([FromRoute] int locationId, [FromRoute] int roomId, [FromRoute] int reviewId)
        {
            return await _roomService.DeleteReview(locationId, roomId, reviewId);
        }

        #endregion

    }
}
