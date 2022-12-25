using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Rooms
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Room> GetAsync(int roomId, int locationId)
        {
            return await GetAsync(_ => _.Id == roomId
                    && !_.IsDelete
                    && _.LocationId == locationId);
        }

        public async Task<Room> GetAsync(int roomId)
        {
            return await GetAsync(_ => _.Id == roomId
                    && !_.IsDelete);
        }

        public IQueryable<Room> GetByFilter(int? locationId)
        {
            return GetQuery(_ => !_.IsDelete
                                 && (!locationId.HasValue 
                                   || _.LocationId == locationId.Value 
                                 )
                            ).OrderByDescending(_ => _.Name);
        }

        public async Task<bool> IsExistsNameRoom(string name, int locationId)
        {
            return await AnyAsync(_ => _.Name == name && _.LocationId == locationId && !_.IsDelete);
        }
    }
}
