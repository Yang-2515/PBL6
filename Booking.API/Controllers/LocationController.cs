using Booking.API.Services;
using Booking.API.ViewModel.Locations.Request;
using Booking.API.ViewModel.Locations.Response;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/locations")]
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

        [HttpGet("all")]
        public async Task<List<LocationInfoResponse>> GetAllLocation([FromQuery] GetLocationFilterRequest request)
        {
            return await _locationService.GetAllLocationAsync(request);
        }

        [HttpGet("{businessId:int}/business")]
        public async Task<List<LocationInfoResponse>> GetLoactionByBusiness([FromRoute]int businessId, [FromQuery] GetLocationInfoByBusinessRequest request)
        {
            return await _locationService.GetLoactionByBusinessAsync(businessId, request);
        }

        [HttpPut]
        public async Task<int> Update([FromBody] UpdateInfoLocationRequest request)
        {
            return await _locationService.UpdateAsync(request);
        }

        [HttpDelete("{id:int}")]
        public async Task<int> Delete([FromRoute] int id)
        {
            return await _locationService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<int> Add([FromBody] AddLocationRequest request)
        {
            return await _locationService.AddAsync(request);
        }

        [HttpGet("{id:int}/utilities")]        
        public async Task<List<UtilityResponse>> GetUtilities([FromRoute] int id)
        {
            return await _locationService.GetUtilitiesAsync(id);
        }
    }
}
