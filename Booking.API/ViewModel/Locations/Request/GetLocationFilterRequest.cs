using Booking.API.ViewModel.Interfaces;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain.Entities;
using System.Linq.Expressions;

namespace Booking.API.ViewModel.Locations.Request
{
    public class GetLocationFilterRequest
    {
        private int _businessId { get; set; }
        public void SetId(int businessId) => _businessId = businessId;
        public int GetId() => _businessId;
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardsId { get; set; }

        public Expression<Func<Location, bool>> GetFilter(GetLocationFilterRequest request)
        {
            return _ => !_.IsDelete
                        && (request.CityId.Equals(null) || request.CityId == _.CityId)
                        && (request.DistrictId.Equals(null) || request.DistrictId == _.DistrictId)
                        && (request.WardsId.Equals(null) || request.WardsId == _.WardsId);
        }

        public Expression<Func<Location, LocationInfoResponse>> GetSelection()
        {
            return _ => new LocationInfoResponse
            {
                Id = _.Id,
                Name = _.Name,
                Description = _.Description,
                Address = _.Address,
                CityId = _.CityId,
                City = _.Wards.District.City.Name,
                DistrictId = _.DistrictId,
                District = _.Wards.District.Name,
                WardsId = _.WardsId,
                Wards = _.Wards.Name,
                IsActive = _.IsActive,
                UtilityResponses = _.Utilitys.Select(_ => new UtilityResponse
                {
                    Id = _.Id,
                    Name = _.Name,
                    Price = _.Price
                }).ToList()
            };
        }
    }
}
