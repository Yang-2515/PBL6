﻿using Booking.API.Services;
using Booking.API.ViewModel.Bookings.Request;
using Booking.API.ViewModel.Bookings.Response;
using Booking.API.ViewModel.Locations.Response;
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

        [HttpPut("{id:int}/extend")]
        public async Task<int> Update([FromRoute] int id, [FromBody] UpdateBookingRequest request)
        {
            return await _bookingService.UpdateAsync(id, request);
        }

        [HttpDelete("{id:int}")]
        public async Task<int> Delete([FromRoute] int id)
        {
            return await _bookingService.DeleteAsync(id);
        }

        [HttpPut("{id:int}/approve")]
        public async Task<int> Approve([FromRoute] int id)
        {
            return await _bookingService.ApproveAsync(id);
        }

        [HttpPut("{id:int}/reject")]
        public async Task<int> Reject([FromRoute] int id)
        {
            return await _bookingService.RejectAsync(id);
        }

        [HttpGet("noti")]
        public async Task<List<NotiResponse>> GetNotiBookingByUser()
        {
            return await _bookingService.GetNotiBookingByUserAsync();
        }
        [HttpPut("test/room/{roomId:int}/booking/{bookingId:int}")]
        public async Task Test([FromRoute] int roomId, [FromRoute] int bookingId)
        {
          await _bookingService.FirstPaymentSuccess(roomId, bookingId);
        }

        [HttpPut("test1/room/{roomId:int}/booking/{bookingId:int}")]
        public async Task Test1([FromRoute] int roomId, [FromRoute] int bookingId)
        {
            await _bookingService.PaymentSuccess(roomId, bookingId);
        }
    }
}
