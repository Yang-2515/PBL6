using Booking.API.Services;
using Booking.API.ViewModel.Location.Response;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/location")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;
        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("cities")]
        public async Task<List<LocationResponse>> GetCities()
        {
            return await _locationService.GetCitiesAsync();
        }

        [HttpGet("cities/{cityId:int}")]
        public async Task<List<LocationResponse>> GetDistrictsByCity([FromRoute] int cityId)
        {
            return await _locationService.GetDistrictsByCityAsync(cityId);
        }

        [HttpGet("districts/{districtId:int}")]
        public async Task<List<LocationResponse>> GetWardsesByDistrict([FromRoute] int districtId)
        {
            return await _locationService.GetWardsesByDistrictAsync(districtId);
        }
    }
}
