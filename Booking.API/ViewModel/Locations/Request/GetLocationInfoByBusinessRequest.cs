using Booking.API.ViewModel.Interfaces;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain.Entities;
using System.Linq.Expressions;

namespace Booking.API.ViewModel.Locations.Request
{
    public class GetLocationInfoByBusinessRequest : IFilter<Location>, ISelection<Location, LocationInfoResponse>
    {
        private string _businessId { get; set; }
        public void SetId(string businessId) => _businessId = businessId;
        public string GetId() => _businessId;

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

        public Expression<Func<Location, bool>> GetFilter()
        {
            return _ => _.BusinessId == GetId() && !_.IsDelete;
        }
    }
}
