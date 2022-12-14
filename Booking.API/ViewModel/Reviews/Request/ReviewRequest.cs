using Booking.API.ViewModel.Reviews.Response;
using Booking.Domain.Entities;
using System.Linq.Expressions;

namespace Booking.API.ViewModel.Reviews.Request
{
    public class ReviewRequest
    {
        public Expression<Func<Review, ReviewResponse>> GetSelection()
        {
            return _ => new ReviewResponse
            {
                Id = _.Id,
                RoomId = _.RoomId,
                Comment = _.Comment,
                ImgId = _.ImgId,
                Rating = _.Rating,
                UserId = _.UserId,
                Name = _.UserName,
                Avatar = _.Avatar,
            };
        }
    }
}
