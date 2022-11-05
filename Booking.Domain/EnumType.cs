using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain
{
    public enum RoomSortType
    {
        Name = 1,
        Price = 2,
        Capacity = 3
    }

    public enum BookingStatus
    {
        Pending = 1,
        Approved = 2,
        Reject = 3
    }
}
