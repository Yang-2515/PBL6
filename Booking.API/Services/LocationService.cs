using Booking.API.Extensions;
using Booking.API.ViewModel.Locations.Request;
using Booking.API.ViewModel.Locations.Response;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Locations;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ErrorMessages = Booking.Domain.Entities.MessageResource;

namespace Booking.API.Services
{
    public class LocationService : ServiceBase
    {
        private readonly ICityRepository _cityRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardsRepository _wardsRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUtilityRepository _utilityRepository;
        private readonly PhotoService _photoService;
        public LocationService(ICityRepository cityRepository
            , IDistrictRepository districtRepository
            , IWardsRepository wardsRepository
            , ILocationRepository locationRepository
            , IUnitOfWork unitOfWork
            , IUtilityRepository utilityRepository
            , IHttpContextAccessor httpContextAccessor
            , PhotoService photoService) : base(httpContextAccessor)
        {
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardsRepository = wardsRepository;
            _unitOfWork = unitOfWork;
            _locationRepository = locationRepository;
            _utilityRepository = utilityRepository;
            _photoService = photoService;
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
            var locations =  await _locationRepository.GetQuery(request.GetFilter(request))
                    .Select(request.GetSelection()).ToListAsync();
            foreach (var item in locations)
            {
                if (item.ImgId != null)
                    item.ImgUrl = await _photoService.GetUrlImage(item.ImgId);
            }
            return locations;
        }
        public async Task<LocationInfoResponse> GetLocationAsync(int id)
        {
            var location = await ValidateLocationAsync(id);
            return new LocationInfoResponse
            {
                Id = location.Id,
                Name = location.Name,
                Description = location.Description,
                Address = location.Address,
                CityId = location.CityId,
                City = location.Wards.District.City.Name,
                DistrictId = location.DistrictId,
                District = location.Wards.District.Name,
                WardsId = location.WardsId,
                Wards = location.Wards.Name,
                IsActive = location.IsActive,
                ImgId = location.ImgId,
                ImgUrl = location.ImgId != null ? await _photoService.GetUrlImage(location.ImgId) : null,
                UtilityResponses = location.Utilitys.Select(_ => new UtilityResponse
                {
                    Id = _.Id,
                    Name = _.Name,
                    Price = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", _.Price)
                }).ToList()
            };
        }

        public async Task<List<UtilityResponse>> GetUtilitiesAsync(int id)
        {
            await ValidateOnGetLocationAsync(id);
            return await _utilityRepository.GetQuery(_ => _.LocationId == id)
                    .Select(_ => new UtilityResponse
                    {
                        Id = _.Id,
                        Name = _.Name,
                        Price = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", _.Price)
                    }).ToListAsync();
        }

        public async Task<int> AddAsync(AddLocationRequest request)
        {
            var location = new Location(request.Name
                , request.Description
                , request.Address
                , GetCurrentUserId().BusinessId
                , request.CityId
                , request.DistrictId
                , request.WardsId
                , request.IsActive
                , request.ImgId
                , GetCurrentUserId().Id);
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

        public async Task<List<LocationInfoResponse>> GetLoactionByBusinessAsync(GetLocationInfoByBusinessRequest request)
        {
            request.SetId(GetCurrentUserId().BusinessId);
            var locations =  await _locationRepository.GetQuery(request.GetFilter())
                                .Select(request.GetSelection())
                                .ToListAsync();
            foreach (var item in locations)
            {
                if (item.ImgId != null)
                    item.ImgUrl = await _photoService.GetUrlImage(item.ImgId);
            }
            return locations;
        }

        public async Task<int> UpdateAsync(UpdateInfoLocationRequest model)
        {
            var location = await ValidateLocationAsync(model.Id);

            if (location.BusinessId != GetCurrentUserId().BusinessId)
            {
                throw new BadRequestException(ErrorMessages.IsNotOwnerLocation);
            }

            location.UpdateInfo(model.Name
                , model.Description
                , model.Address
                , model.CityId
                , model.DistrictId
                , model.WardsId
                , model.IsActive
                , model.ImgId);
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
            var location = await ValidateLocationAsync(id);

            if (location.BusinessId != GetCurrentUserId().BusinessId)
            {
                throw new BadRequestException(ErrorMessages.IsNotOwnerLocation);
            }

            location.Remove();
            await _unitOfWork.SaveChangeAsync();

            return location.Id;
        }
        public async Task<Location> ValidateLocationAsync(int id)
        {
            var location = await _locationRepository.GetAsync(id);
            if (location == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundLocation);

            return location;
        }

        public async Task ValidateOnGetLocationAsync(int id)
        {
            if (!await _locationRepository.AnyAsync(id))
                throw new BadRequestException(ErrorMessages.IsNotFoundLocation);
        }
    }
}
