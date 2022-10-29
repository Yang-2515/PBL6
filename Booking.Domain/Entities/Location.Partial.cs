using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Location
    {
        public void UpdateInfo(string name, string description, string address, int cityId, int districtId, int wardsId)
        {
            Name = name;
            Description = description;
            Address = address;  
            CityId = cityId;
            DistrictId = districtId;
            WardsId = wardsId;
        }

        public void Remove()
        {
            IsDelete = true;
        }
    }
}
