using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class BookingUtility
    {
        public BookingUtility(int? bookingId, int? utilityId, string name, int price)
        {
            BookingId = bookingId;
            UtilityId = utilityId;
            Name = name;
            Price = price;
        }
    }
}
