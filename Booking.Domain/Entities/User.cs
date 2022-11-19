using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class User
    {
        public User(string id, string name, string businessId)
        {
            Id = id;
            Name = name;
            BusinessId = businessId;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }

        public string BusinessId { get; private set; }
    }
}
