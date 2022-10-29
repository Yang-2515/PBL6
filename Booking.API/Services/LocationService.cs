using Booking.API.ViewModel.Location.Request;
using Booking.API.ViewModel.Location.Response;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Services
{
    public class LocationService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardsRepository _wardsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LocationService(ICityRepository cityRepository, IDistrictRepository districtRepository, IWardsRepository wardsRepository, IUnitOfWork unitOfWork)
        {
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardsRepository = wardsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LocationResponse>> GetCitiesAsync()
        {
            var request = new LocationRequest();
            return await _cityRepository.GetQuery(request.GetFilterByCity()).Select(request.GetSelectionByCity()).ToListAsync();
        }

        public async Task<List<LocationResponse>> GetDistrictsByCityAsync(int cityId)
        {
            var request = new LocationRequest();
            return await _districtRepository.GetQuery(request.GetFilterByDistrict(cityId)).Select(request.GetSelectionByDistrict()).ToListAsync();
        }
        public async Task<List<LocationResponse>> GetWardsesByDistrictAsync(int cityId)
        {
            var request = new LocationRequest();
            return await _wardsRepository.GetQuery(request.GetFilterByWards(cityId)).Select(request.GetSelectionByWard()).ToListAsync();
        }
    }
}
