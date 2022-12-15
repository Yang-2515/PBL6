using Booking.API.ViewModel.Bookings.Response;
using Booking.API.ViewModel.Interfaces;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain;
using System.Linq.Expressions;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.API.ViewModel.Bookings.Request
{
    public class GetBookingRequest : ISelection<BookingEntity, GetBookingResponse>
    {
        public Expression<Func<BookingEntity, bool>> GetFilterByUser(string userId, BookingStatus? status)
        {
            return _ => _.UserId == userId && !_.IsDelete
                        && (status == 0 || _.Status == status);
        }

        public Expression<Func<BookingEntity, bool>> GetFilterByBusiness(string businessId)
        {
            return _ => _.BusinessId.Equals(businessId) && !_.IsDelete;
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
                MonthNumber = _.MonthNumber,
                Status = Enum.GetName(_.Status),
                Utilities = _.BookingUtilities
                                        .Select(_ => new UtilityResponse
                                        {
                                            Name = _.Name,
                                            Price = _.Price
                                        }).ToList()
            };
        }
    }
}
