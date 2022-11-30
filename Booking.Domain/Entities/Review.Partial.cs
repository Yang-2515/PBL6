using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Review
    {
        public Review()
        {

        }
        public Review(int rating
                      , string comment
                      , string? imgId
                      , string userId
                      , string name
                      , string? avatar)
        {
            Rating = rating;
            Comment = comment;
            ImgId = imgId;
            UserId = userId;
            UserName = name;
            Avatar = avatar;
        }
        
        public void Update(int rating
                      , string comment
                      , string? imgId)
        {
            Rating = rating;
            Comment = comment;
            ImgId = imgId;
            UpdateOn = DateTime.UtcNow;
        }

    }
}
