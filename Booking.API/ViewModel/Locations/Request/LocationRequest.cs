using Booking.API.ViewModel.Locations.Response;
using Booking.Domain.Entities;
using System.Linq.Expressions;

namespace Booking.API.ViewModel.Locations.Request
{
    public class LocationRequest
    {
		public Expression<Func<City, bool>> GetFilterByCity()
		{
			return _ => true;
		}
		public Expression<Func<City, LocationResponse>> GetSelectionByCity()
		{
			return _ => new LocationResponse()
			{
				Id = _.Id,
				Name = _.Name
			};
		}
		public Expression<Func<District, bool>> GetFilterByDistrict(int cityId)
		{
			return _ => _.CityId == cityId;
		}
		public Expression<Func<District, LocationResponse>> GetSelectionByDistrict()
		{
			return _ => new LocationResponse()
			{
				Id = _.Id,
				Name = _.Name
			};
		}
		public Expression<Func<Wards, bool>> GetFilterByWards(int districtId)
		{
			return _ => _.DistrictId == districtId;
		}
		public Expression<Func<Wards, LocationResponse>> GetSelectionByWard()
		{
			return _ => new LocationResponse()
			{
				Id = _.Id,
				Name = _.Name
			};
		}
	}
}
