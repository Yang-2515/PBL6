using Booking.API.ViewModel.Rooms.Request;
using Booking.API.ViewModel.Rooms.Response;
using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Locations;
using Booking.Domain.Interfaces.Repositories.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IRoomRepository roomRepository
                          , ILocationRepository locationRepository
                          , IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RoomBasicInfoResponse>>GetByFilter(int locationId, RoomBasicInfoRequest request)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadHttpRequestException("Không tồn tại vị trí");
            var rooms = _roomRepository.GetByFilter(locationId
                                                    , request.Name
                                                    , request.FromCapacity
                                                    , request.ToCapacity
                                                    , request.FromPrice
                                                    , request.ToPrice);
            if (request.Sort.HasValue)
            {
                switch (request.Sort)
                {
                    case RoomSortType.Name:
                        rooms.OrderBy(_ => _.Name);
                        break;
                    case RoomSortType.Price:
                        rooms.OrderBy(_ => _.Price);
                        break;
                    case RoomSortType.Capacity:
                        rooms.OrderBy(_ => _.Capacity);
                        break;
                    default:
                        rooms.OrderBy(_ => _.Name);
                        break;
                }
            }
            return await rooms.Select(new RoomBasicInfoRequest().GetSelection()).ToListAsync();
        }

        public async Task<bool> CreateAsync(int locationId, CreateRoomRequest request)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadHttpRequestException("Không tồn tại vị trí");
            var room = new Room(locationId
                                , request.Name
                                , request.BusinessId
                                , request.Capacity
                                , request.Price);
            await _roomRepository.InsertAsync(room);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(int locationId, int roomId, UpdateRoomRequest request)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadHttpRequestException("Không tồn tại vị trí");
            var room = await _roomRepository.GetAsync(_ => _.Id == roomId);
            if (room == null)
                throw new BadHttpRequestException("Không tồn tại phòng");
            room.Update(locationId
                        , request.Name != null ? request.Name : room.Name
                        , request.BusinessId.HasValue ? request.BusinessId.Value : room.BusinessId
                        , request.Capacity.HasValue ? request.Capacity.Value : room.Capacity
                        , request.Price.HasValue ? request.Price.Value : room.Price);
            await _roomRepository.UpdateAsync(room);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int locationId, int roomId)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadHttpRequestException("Không tồn tại vị trí");
            var room = await _roomRepository.GetAsync(_ => _.Id == roomId);
            if (room == null)
                throw new BadHttpRequestException("Không tồn tại phòng");
            await _roomRepository.RemoveAsync(room);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public async Task<bool> ValidLocation(int locationId)
        {
            var location = await _locationRepository.GetAsync(_ => _.Id == locationId);
            if (location == null)
                return false;
            return true;
        }
    }
}
