using Booking.API.ViewModel.Bookings.Response;
using Booking.API.ViewModel.Interfaces;
using Booking.API.ViewModel.Locations.Response;
using System.Linq.Expressions;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.API.ViewModel.Bookings.Request
{
    public class GetBookingRequest : ISelection<BookingEntity, GetBookingResponse>
    {
        public Expression<Func<BookingEntity, bool>> GetFilterByUser(int userId)
        {
            return _ => _.UserId == userId && !_.IsDelete;
        }

        public Expression<Func<BookingEntity, bool>> GetFilterByBusiness(int businessId)
        {
            return _ => _.BusinessId == businessId && !_.IsDelete;
        }

        public Expression<Func<BookingEntity, GetBookingResponse>> GetSelection()
        {
            return _ => new GetBookingResponse
            {
                Id = _.Id,
                UserId = _.UserId,
                UserName = _.UserName,
                RoomId = _.RoomId,
                RoomName = _.Room.Name,
                StartDay = _.StartDay,
                FinishDay = _.FinishDay,
                Status = _.Status.ToString(),
                Utilities = _.BookingUtilities.Select(_ => _.Utility)
                                        .Select(_ => new UtilityResponse
                                        {
                                            Id = _.Id,
                                            Name = _.Name,
                                            Price = _.Price
                                        }).ToList()
            };
        }
    }
}
