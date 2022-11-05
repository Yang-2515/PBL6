namespace Booking.API.ViewModel.Locations.Response
{
    public class LocationInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int WardsId { get; set; }
        public string Wards { get; set; }
        public bool IsActive { get; set; }
        public List<UtilityResponse> UtilityResponses { get; set; }
    }
}
