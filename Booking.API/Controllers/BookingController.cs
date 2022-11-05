using Booking.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /*[HttpGet]
        public async Task*/
    }
}
