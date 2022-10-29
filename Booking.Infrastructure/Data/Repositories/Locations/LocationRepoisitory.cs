﻿using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Repositories.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Locations
{
    public class LocationRepoisitory : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepoisitory(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Location> GetAsync(int id)
        {
            return await GetAsync(_ => _.Id == id);
        }
    }
}
