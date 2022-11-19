using Booking.Domain.Interfaces.Repositories.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public partial class Room
    {
        public Room()
        {
            Reviews = new List<Review>();
        }
        public Room(int locationId
                    , string name
                    , string businessId
                    , int capacity
                    , int price)
        {
            Name = name;
            LocationId = locationId;
            BusinessId = businessId;
            Capacity = capacity;
            Price = price;
        }

        public void Update(string name
                           , int capacity
                           , int price)
        {
            Name = name;
            Capacity = capacity;
            Price = price;
        }

        public void AddReview(int rating
                              , string comment
                              , string imgUrl
                              , string userId)
        {
            Reviews.Add(new Review(rating
                                   , comment
                                   , imgUrl
                                   , userId
                                   , Id));
        }

        public void RemoveReview(Review review)
        {
            Reviews.Remove(review);
        }
        
        public void Remove()
        {
            IsDelete = true;
            Reviews.Clear();
        }

    }
}
