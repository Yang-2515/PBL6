using Booking.Domain.Entities;

namespace Booking.Domain.Interfaces.Repositories.Rooms
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        IQueryable<Room> GetByFilter(int? locationId);
        Task<Room> GetAsync(int roomId);
        Task<bool> IsExistsNameRoom(string name, int locationId);
    }
}
