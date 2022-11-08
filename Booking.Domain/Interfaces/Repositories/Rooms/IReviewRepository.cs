using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Repositories.Rooms
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        IQueryable<Review> GetAll(int roomId);
    }
}
