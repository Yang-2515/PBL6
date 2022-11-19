using Booking.Domain.DomainEvents.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Location
    {
        public Location(string name, string description, string address, string businessId, int cityId, int districtId, int wardsId, bool isActive)
        {
            Name = name;
            Description = description;
            Address = address;
            BusinessId = businessId;
            CityId = cityId;
            DistrictId = districtId;
            WardsId = wardsId;
            IsActive = isActive;
            Utilitys = new List<Utility>();
            CreateOn = DateTime.UtcNow;
        }

        public void UpdateInfo(string name, string description, string address, int cityId, int districtId, int wardsId, bool isActive)
        {
            Name = name;
            Description = description;
            Address = address;  
            CityId = cityId;
            DistrictId = districtId;
            WardsId = wardsId;
            IsActive = isActive;
            UpdateOn = DateTime.UtcNow;
            Utilitys.Clear();
        }

        public void Remove()
        {
            IsDelete = true;
            Utilitys.Clear();

            this.AddEvent(new DeleteLocationDomainEvent(Id));
        }

        public void AddUtility(string name, int price)
        {
            this.Utilitys.Add(new Utility(name, price));
        }
    }
}
