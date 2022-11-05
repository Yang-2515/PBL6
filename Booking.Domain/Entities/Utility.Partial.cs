using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Utility
    {
        public Utility(string name, int price)
        {
            Name = name;
            Price = price;
            BookingUtilities = new List<BookingUtility>();
            CreateOn = DateTime.UtcNow;
        }
    }
}
