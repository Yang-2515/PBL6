using Booking.API.ViewModel.Reviews.Request;
using Booking.API.ViewModel.Reviews.Response;
using Booking.API.ViewModel.Rooms.Request;
using Booking.API.ViewModel.Rooms.Response;
using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Locations;
using Booking.Domain.Interfaces.Repositories.Rooms;
using Microsoft.EntityFrameworkCore;
using ErrorMessages = Booking.Domain.Entities.MessageResource;

namespace Booking.API.Services
{
    public class RoomService : ServiceBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IRoomRepository roomRepository
                          , ILocationRepository locationRepository
                          , IReviewRepository reviewRepository
                          , IUnitOfWork unitOfWork
                          , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _locationRepository = locationRepository;
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RoomBasicInfoResponse>> GetByFilter(int locationId, RoomBasicInfoRequest request)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundLocation);
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

        public async Task<List<ReviewResponse>> GetAllReviewAsync(int roomId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundRoom);
            return room.Reviews
                    .AsQueryable()
                    .Select(new ReviewRequest().GetSelection())
                    .ToList();
        }

        public async Task<int> CreateAsync(AddRoomRequest request)
        {
            var isOwner = await _locationRepository.IsOwnerAsync(request.LocationId, GetCurrentUserId().BusinessId);
            if (!isOwner)
                throw new BadHttpRequestException(ErrorMessages.IsNotOwnerLocation);

            var isExistsName = await _roomRepository.IsExistsNameRoom(request.Name);
            if(isExistsName)
                throw new BadHttpRequestException(ErrorMessages.IsExistsNameRoom);

            var room = new Room(request.LocationId
                                , request.Name
                                , request.BusinessId
                                , request.Capacity
                                , request.Price);
            await _roomRepository.InsertAsync(room);
            await _unitOfWork.SaveChangeAsync();

            return room.Id;
        }

        public async Task<bool> AddReviewAsync(int roomId, AddReviewRequest request)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundRoom);
            
            room.AddReview(request.Rating, request.Comment, request.ImgUrl, GetCurrentUserId().Id);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewRequest request)
        {
            var review = await ValidateOnGetReview(reviewId);
            if (review.UserId != GetCurrentUserId().Id)
                throw new BadHttpRequestException(ErrorMessages.IsNotOwnerReview);

            review.Update(request.Rating, request.Comment, request.ImgUrl);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateAsync(int roomId, UpdateRoomRequest request)
        {
            var room = await _roomRepository.GetAsync(_ => _.Id == roomId 
                            && !_.IsDelete);
            if (room == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundRoom);

            if (room.BusinessId != GetCurrentUserId().BusinessId)
                throw new BadHttpRequestException(ErrorMessages.IsNotOwnerRoom);

            room.Update( request.Name != null ? request.Name : room.Name
                        , request.Capacity.HasValue ? request.Capacity.Value : room.Capacity
                        , request.Price.HasValue ? request.Price.Value : room.Price);
            await _roomRepository.UpdateAsync(room);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<int> DeleteAsync(int roomId)
        {
            var room = await ValidateOnGetRoom(roomId);
            if (room.BusinessId != GetCurrentUserId().BusinessId)
                throw new BadHttpRequestException(ErrorMessages.IsNotOwnerRoom);
            room.Remove();
            await _unitOfWork.SaveChangeAsync();
            return room.Id;
        }

        public async Task<bool> DeleteReviewAsync(int roomId, int reviewId)
        {
            var room = await ValidateOnGetRoom(roomId);
            var review = await ValidateOnGetReview(reviewId);
            if (review.UserId != GetCurrentUserId().Id)
                throw new BadHttpRequestException(ErrorMessages.IsNotOwnerReview);

            room.RemoveReview(review);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> ValidLocation(int locationId)
        {
            return await _locationRepository.AnyAsync(_ => _.Id == locationId && !_.IsDelete);
        }

        public async Task<Review> ValidateOnGetReview(int reviewId)
        {
            var review = await _reviewRepository.GetAsync(_ => _.Id == reviewId && !_.IsDelete);
            if (review == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundReview);
            return review;
        }

        public async Task<Room> ValidateOnGetRoom(int roomId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadHttpRequestException(ErrorMessages.IsNotFoundRoom);
            return room;
        }
    }
}
