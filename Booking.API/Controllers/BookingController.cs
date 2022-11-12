using Booking.API.Services;
using Booking.API.ViewModel.Bookings.Request;
using Booking.API.ViewModel.Bookings.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/bookings")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("current-user")]
        public async Task<List<GetBookingResponse>> GetBookingByUser()
        {
            return await _bookingService.GetBookingByUserAsync();
        }

        [HttpGet("business")]
        public async Task<List<GetBookingResponse>> GetBookingByBusiness()
        {
            return await _bookingService.GetBookingByBusinessAsync();
        }

        [HttpPost]
        public async Task<int> Add([FromBody] AddBookingRequest request)
        {
            return await _bookingService.AddAsync(request);
        }

        [HttpPut("{id:int}")]
        public async Task<int> Update([FromRoute] int id, [FromBody] UpdateBookingRequest request)
        {
            return await _bookingService.UpdateAsync(id, request);
        }

        [HttpDelete("{id:int}")]
        public async Task<int> Delete([FromRoute] int id)
        {
            return await _bookingService.DeleteAsync(id);
        }
    }
}
