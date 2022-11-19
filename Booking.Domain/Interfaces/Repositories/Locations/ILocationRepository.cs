using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Repositories.Locations
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<Location> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        Task<bool> IsOwnerAsync(int id, string businessId);
    }
}
