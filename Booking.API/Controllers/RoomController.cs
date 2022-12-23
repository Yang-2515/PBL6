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
    [Route("api/booking/rooms")]
    
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("/api/booking/locations/{locationId:int}/rooms/all")]
        public async Task<List<RoomBasicInfoResponse>> GetAll([FromRoute] int locationId)
        {
            return await _roomService.GetByFilter(locationId);
        }

        [HttpGet("top-7")]
        public async Task<List<RoomBasicInfoResponse>> GetTopRoom()
        {
            return await _roomService.GetTopRoomAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task<int> CreateRoom([FromBody] AddRoomRequest request)
        {
            return await _roomService.CreateAsync(request);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<bool> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomRequest request)
        {
            return await _roomService.UpdateAsync(id, request);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<int> DeleteRoom([FromRoute] int id)
        {
            return await _roomService.DeleteAsync(id);
        }

        [HttpGet("{id:int}/detail")]
        public async Task<RoomBasicInfoResponse> GetRoom([FromRoute] int id)
        {
            return await _roomService.GetAsync(id);
        }

        #region Review

        [HttpGet("{roomId:int}/reviews")]
        public async Task<List<ReviewResponse>> GetReview([FromRoute] int roomId)
        {
            return await _roomService.GetAllReviewAsync(roomId);
        }
        [Authorize]
        [HttpPost("{roomId:int}/reviews")]
        public async Task<bool> AddReview([FromRoute] int roomId, [FromBody] AddReviewRequest request)
        {
            return await _roomService.AddReviewAsync(roomId, request);
        }
        [Authorize]
        [HttpDelete("{roomId:int}/reviews/{reviewId}")]
        public async Task<bool> DeleteReview([FromRoute] int roomId, [FromRoute] int reviewId)
        {
            return await _roomService.DeleteReviewAsync(roomId, reviewId);
        }
        [Authorize]
        [HttpPut("reviews/{id:int}")]
        public async Task<bool> UpdateReview([FromRoute] int id, [FromBody] UpdateReviewRequest request)
        {
            return await _roomService.UpdateReviewAsync(id, request);
        }

        #endregion

    }
}
