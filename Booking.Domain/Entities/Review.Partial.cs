using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Review
    {
        public Review(int rating
                      , string comment
                      , string imgUrl
                      , int userId
                      , int roomId)
        {
            Update(rating, comment, imgUrl, userId, roomId);
        }
        
        public void Update(int rating
                      , string comment
                      , string imgUrl
                      , int userId
                      , int roomId)
        {
            Rating = rating;
            Comment = comment;
            ImgUrl = imgUrl;
            UserId = userId;
            RoomId = roomId;
        }

    }
}
