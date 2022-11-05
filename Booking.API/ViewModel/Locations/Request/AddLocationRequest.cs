namespace Booking.API.ViewModel.Locations.Request
{
    public class AddLocationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardsId { get; set; }
        public bool IsActive { get; set; }
        public List<UtilityRequest> Utilities { get; set; }
    }
}
