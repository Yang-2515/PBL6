using Booking.API.ViewModel.Rooms.Response;
using Booking.Domain;
using Booking.Domain.Entities;
using System.Linq.Expressions;

namespace Booking.API.ViewModel.Rooms.Request
{
    public class RoomBasicInfoRequest
    {
		public string? Name { get; set; }
		public int? FromCapacity { get; set; }
		public int? ToCapacity { get; set; }
		public int? FromPrice { get; set; }
        public int? ToPrice { get; set; }
        public RoomSortType? Sort { get; set; }
        public Expression<Func<Room, RoomBasicInfoResponse>> GetSelection()
		{
			return _ => new RoomBasicInfoResponse()
			{
				Id = _.Id,
				Name = _.Name,
				Price = _.Price,
				Capacity = _.Capacity
			};
		}
	}
}
