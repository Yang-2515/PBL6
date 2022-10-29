namespace Booking.API.ViewModel.Locations.Request
{
    public class UpdateInfoLocationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardsId { get; set; }
    }
}
