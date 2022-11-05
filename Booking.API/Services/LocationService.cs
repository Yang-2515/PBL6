using Booking.API.ViewModel.Locations.Request;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Locations;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Services
{
    public class LocationService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardsRepository _wardsRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUtilityRepository _utilityRepository;
        public LocationService(ICityRepository cityRepository
            , IDistrictRepository districtRepository
            , IWardsRepository wardsRepository
            , ILocationRepository locationRepository
            , IUnitOfWork unitOfWork
            , IUtilityRepository utilityRepository)
        {
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardsRepository = wardsRepository;
            _unitOfWork = unitOfWork;
            _locationRepository = locationRepository;
            _utilityRepository = utilityRepository;
        }

        public async Task<List<LocationResponse>> GetCitiesAsync()
        {
            var request = new LocationRequest();
            return await _cityRepository.GetQuery(request.GetFilterByCity())
                        .Select(request.GetSelectionByCity())
                        .ToListAsync();
        }

        public async Task<List<LocationResponse>> GetDistrictsByCityAsync(int cityId)
        {
            var request = new LocationRequest();
            return await _districtRepository.GetQuery(request.GetFilterByDistrict(cityId))
                        .Select(request.GetSelectionByDistrict())
                        .ToListAsync();
        }

        public async Task<List<LocationResponse>> GetWardsesByDistrictAsync(int cityId)
        {
            var request = new LocationRequest();
            return await _wardsRepository.GetQuery(request.GetFilterByWards(cityId))
                        .Select(request.GetSelectionByWard())
                        .ToListAsync();
        }

        public async Task<List<LocationInfoResponse>> GetAllLocationAsync(GetLocationFilterRequest request)
        {
            return await _locationRepository.GetQuery(request.GetFilter(request))
                    .Select(request.GetSelection()).ToListAsync();
        }

        public async Task<List<UtilityResponse>> GetUtilitiesAsync(int id)
        {
            await ValidateOnGetLocationAsync(id);
            return await _utilityRepository.GetQuery(_ => _.LocationId == id)
                    .Select(_ => new UtilityResponse
                    {
                        Id = _.Id,
                        Name = _.Name,
                        Price = _.Price
                    }).ToListAsync();
        }

        public async Task<int> AddAsync(AddLocationRequest request)
        {
            var location = new Location(request.Name
                , request.Description
                , request.Address
                , 1
                , request.CityId
                , request.DistrictId
                , request.WardsId
                , request.IsActive);
            if (request.Utilities.Any())
            {
                foreach(var item in request.Utilities)
                {
                    location.AddUtility(item.Name, item.Price);
                }
            }

            await _locationRepository.InsertAsync(location);
            await _unitOfWork.SaveChangeAsync();
            return location.Id;
            
        }

        public async Task<List<LocationInfoResponse>> GetLoactionByBusinessAsync(int businessId, GetLocationInfoByBusinessRequest request)
        {
            request.SetId(businessId);
            return await _locationRepository.GetQuery(request.GetFilter())
                        .Select(request.GetSelection())
                        .ToListAsync();
        }

        public async Task<int> UpdateAsync(UpdateInfoLocationRequest model)
        {
            var location = await GetLocationAsync(model.Id);

            location.UpdateInfo(model.Name
                , model.Description
                , model.Address
                , model.CityId
                , model.DistrictId
                , model.WardsId
                , model.IsActive);
            if (model.Utilities.Any())
            {
                foreach(var item in model.Utilities)
                {
                    location.AddUtility(item.Name, item.Price);
                }
            }
            await _locationRepository.UpdateAsync(location);
            await _unitOfWork.SaveChangeAsync();

            return location.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var location = await GetLocationAsync(id);

            location.Remove();
            await _unitOfWork.SaveChangeAsync();

            return location.Id;
        }
        public async Task<Location> GetLocationAsync(int id)
        {
            var location = await _locationRepository.GetAsync(id);
            if (location == null)
                throw new BadHttpRequestException("Business not found");

            return location;
        }

        public async Task ValidateOnGetLocationAsync(int id)
        {
            if (! await _locationRepository.AnyAsync(id))
                throw new BadHttpRequestException("Business not found");
        }
    }
}
