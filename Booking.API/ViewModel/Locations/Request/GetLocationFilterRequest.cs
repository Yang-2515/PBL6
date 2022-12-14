﻿using Booking.API.ViewModel.Interfaces;
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
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }

        public Expression<Func<Location, bool>> GetFilter(GetLocationFilterRequest request)
        {
            return _ => !_.IsDelete
                        && (!request.CityId.HasValue || request.CityId == _.CityId)
                        && (!request.DistrictId.HasValue || request.DistrictId == _.DistrictId)
                        && (!request.WardsId.HasValue || request.WardsId == _.WardsId)
                        && (!(request.MaxPrice.HasValue && request.MinPrice.HasValue) || _.Rooms.Any(_ => _.Price <= request.MaxPrice && _.Price >= request.MinPrice))
                        && (!request.MaxPrice.HasValue || _.Rooms.Any(_ => _.Price <= request.MaxPrice))
                        && (!request.MinPrice.HasValue || _.Rooms.Any(_ => _.Price >= request.MinPrice));
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
                ImgId = _.ImgId,
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
