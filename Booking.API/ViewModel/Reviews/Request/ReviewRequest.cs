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
                RoomId = _.RoomId,
                Comment = _.Comment,
                ImgUrl = _.ImgUrl,
                Rating = _.Rating,
                UserId = _.UserId
            };
        }
    }
}
