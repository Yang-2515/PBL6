using Booking.API.Extensions;
using Booking.API.ViewModel.Reviews.Request;
using Booking.API.ViewModel.Reviews.Response;
using Booking.API.ViewModel.Rooms.Request;
using Booking.API.ViewModel.Rooms.Response;
using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Locations;
using Booking.Domain.Interfaces.Repositories.Rooms;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ErrorMessages = Booking.Domain.Entities.MessageResource;

namespace Booking.API.Services
{
    public class RoomService : ServiceBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PhotoService _photoService;
        private readonly IBookingRepository _bookingRepo;

        public RoomService(IRoomRepository roomRepository
                          , ILocationRepository locationRepository
                          , IReviewRepository reviewRepository
                          , IUnitOfWork unitOfWork
                          , IHttpContextAccessor httpContextAccessor
                          , PhotoService photoService
                          , IBookingRepository bookingRepo) : base(httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _locationRepository = locationRepository;
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _bookingRepo = bookingRepo;
        }

        public async Task<List<RoomBasicInfoResponse>> GetTopRoomAsync()
        {
            var rooms = await _roomRepository.GetQuery(_ => !_.IsDelete)
                                        .Select(_ => new
                                        {
                                            Rating = _.Reviews.Select(_ => _.Rating).Count() == 0 ? 0 : _.Reviews.Select(_ => _.Rating).Sum() / _.Reviews.Select(_ => _.Rating).Count(),
                                            Room = _
                                        })
                                        .OrderByDescending(_ => _.Rating)
                                        .Take(7)
                                        .Select(_ => new RoomBasicInfoResponse()
                                        {
                                            Id = _.Room.Id,
                                            Name = _.Room.Name,
                                            Price = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", _.Room.Price),
                                            Capacity = _.Room.Capacity,
                                            ImgId = _.Room.ImgId,
                                            AvailableDay = _.Room.AvailableDay,
                                            LocationId = _.Room.LocationId,
                                            Rating = _.Rating
                                        }).ToListAsync();
            foreach (var item in rooms)
            {
                item.ImgUrl = item.ImgId != null ? await _photoService.GetUrlImage(item.ImgId) : null;
            }
            return rooms;
        }

        public async Task<List<RoomBasicInfoResponse>> GetByFilter(int locationId)
        {
            var isValidLocation = await ValidLocation(locationId);
            if (!isValidLocation)
                throw new BadRequestException(ErrorMessages.IsNotFoundLocation);
            var rooms = _roomRepository.GetByFilter(locationId);
            var roomResponses =  await rooms.Select(new RoomBasicInfoRequest().GetSelection()).ToListAsync();
            foreach (var item in roomResponses)
            {
                if (item.ImgId != null)
                    item.ImgUrl = await _photoService.GetUrlImage(item.ImgId);
            }
            return roomResponses;
        }

        public async Task<RoomBasicInfoResponse> GetAsync(int id)
        {
            var room = await ValidateOnGetRoom(id);
            var reviews = room.Reviews.Select(_ => _.Rating);
            return new RoomBasicInfoResponse
            {
                Id = room.Id,
                Name = room.Name,
                Price = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", room.Price),
                Capacity = room.Capacity,
                AvailableDay = room.AvailableDay,
                ImgId = room.ImgId,
                ImgUrl = room.ImgId != null ? await _photoService.GetUrlImage(room.ImgId) : null,
                Rating = reviews.Count() == 0 ? 0 : reviews.Sum() / reviews.Count()
            };
        }

        public async Task<List<ReviewResponse>> GetAllReviewAsync(int roomId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundRoom);
            var reviews =  room.Reviews
                                .AsQueryable()
                                .Select(new ReviewRequest().GetSelection())
                                .ToList();
            foreach (var item in reviews)
            {
                if(item.ImgId != null)
                    item.ImgUrl = await _photoService.GetUrlImage(item.ImgId);
            }
            return reviews;
        }

        public async Task<int> CreateAsync(AddRoomRequest request)
        {
            if (request.AvailableDay.Day < DateTime.UtcNow.Day)
                throw new BadRequestException(ErrorMessages.IsNotValidAvailableDay);

            var isOwner = await _locationRepository.IsOwnerAsync(request.LocationId, GetCurrentUserId().BusinessId);
            if (!isOwner)
                throw new BadRequestException(ErrorMessages.IsNotOwnerLocation);
            var isExistsName = await _roomRepository.IsExistsNameRoom(request.Name, request.LocationId);
            if(isExistsName)
                throw new BadRequestException(ErrorMessages.IsExistsNameRoom);
 
            var room = new Room(request.LocationId
                                , request.Name
                                , GetCurrentUserId().BusinessId
                                , request.Capacity
                                , request.Price
                                , request.ImgId
                                , request.AvailableDay);
            await _roomRepository.InsertAsync(room);
            await _unitOfWork.SaveChangeAsync();

            return room.Id;
        }

        public async Task<bool> AddReviewAsync(int roomId, AddReviewRequest request)
        {
            var isExistUserReview = await _reviewRepository.AnyAsync(_ => _.UserId == GetCurrentUserId().Id && _.RoomId == roomId && !_.IsDelete);
            if (isExistUserReview)
                throw new BadRequestException(ErrorMessages.IsExistsUserReview);
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundRoom);
            
            room.AddReview(request.Rating
                , request.Comment
                , request.ImgId
                , GetCurrentUserId().Id
                , GetCurrentUserId().Name
                , GetCurrentUserId().Avatar);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewRequest request)
        {
            var review = await ValidateOnGetReview(reviewId);
            if (review.UserId != GetCurrentUserId().Id)
                throw new BadRequestException(ErrorMessages.IsNotOwnerReview);

            review.Update(request.Rating, request.Comment, request.ImgId);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateAsync(int roomId, UpdateRoomRequest request)
        {
            var room = await _roomRepository.GetAsync(_ => _.Id == roomId 
                            && !_.IsDelete);
            if (room == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundRoom);

            if (room.BusinessId != GetCurrentUserId().BusinessId)
                throw new BadRequestException(ErrorMessages.IsNotOwnerRoom);

            if(room.Name != request.Name)
            {
                var isExistsName = await _roomRepository.IsExistsNameRoom(request.Name, room.LocationId);
                if (isExistsName)
                    throw new BadRequestException(ErrorMessages.IsExistsNameRoom);
            }
           
            room.Update( request.Name
                        ,request.Capacity
                        , request.Price
                        , request.ImgId
                        , request.AvailableDay);
            await _roomRepository.UpdateAsync(room);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<int> DeleteAsync(int roomId)
        {
            var room = await ValidateOnGetRoom(roomId);
            var isHired = await _bookingRepo.IsHiredAsync(roomId);
            if(isHired)
                throw new BadRequestException(ErrorMessages.RoomIsHired);
            if (room.BusinessId != GetCurrentUserId().BusinessId)
                throw new BadRequestException(ErrorMessages.IsNotOwnerRoom);
            room.Remove();
            await _unitOfWork.SaveChangeAsync();
            return room.Id;
        }

        public async Task<bool> DeleteReviewAsync(int roomId, int reviewId)
        {
            var room = await ValidateOnGetRoom(roomId);
            var review = await ValidateOnGetReview(reviewId);
            if (review.UserId != GetCurrentUserId().Id)
                throw new BadRequestException(ErrorMessages.IsNotOwnerReview);

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
                throw new BadRequestException(ErrorMessages.IsNotFoundReview);
            return review;
        }

        public async Task<Room> ValidateOnGetRoom(int roomId)
        {
            var room = await _roomRepository.GetAsync(roomId);
            if (room == null)
                throw new BadRequestException(ErrorMessages.IsNotFoundRoom);
            return room;
        }
    }
}
