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
                    , int price
                    , string? imgId
                    , DateTime availableDay)
        {
            Name = name;
            LocationId = locationId;
            BusinessId = businessId;
            Capacity = capacity;
            Price = price;
            ImgId = imgId;
            AvailableDay = availableDay;
            CreateOn = DateTime.UtcNow;
        }

        public void Update(string name
                           , int capacity
                           , int price
                           , string imgId
                           , DateTime? availableDay)
        {
            Name = name;
            Capacity = capacity;
            Price = price;
            ImgId = imgId;
            AvailableDay = availableDay;
            UpdateOn = DateTime.UtcNow;
        }

        public void AddReview(int rating
                              , string comment
                              , string? imgId
                              , string userId
                              , string name
                              , string? avatar)
        {
            Reviews.Add(new Review(rating
                                   , comment
                                   , imgId
                                   , userId
                                   , name
                                   , avatar));
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

        public void HandleBookingSuccess(int monthNumber, DateTime? startDay)
        {
            if(!startDay.HasValue)
            {
                var dateTime = AvailableDay.Value.AddMonths(monthNumber);
                AvailableDay = dateTime;
            }
            else
            {
                var dateTime = startDay.Value.AddMonths(monthNumber);
                AvailableDay = dateTime;
            }
            
            IsBooked = true;
        }
    }
}
