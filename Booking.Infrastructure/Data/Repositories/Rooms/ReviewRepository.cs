using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Rooms
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {

        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<Review> GetAll(int roomId)
        {
            return GetQuery(_ => _.RoomId == roomId);
        }

        public async Task<Review> GetAsync(int roomId, int reviewId)
        {
            return await GetAsync(_ => _.RoomId == roomId && _.Id == reviewId);
        }
    }
}
