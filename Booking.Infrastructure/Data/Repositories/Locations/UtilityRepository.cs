using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Locations
{
    public class UtilityRepository : GenericRepository<Utility>, IUtilityRepository
    {
        public UtilityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
