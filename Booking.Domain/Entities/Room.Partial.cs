﻿using System;
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
                    , int businessId
                    , int capacity
                    , int price)
        {
            Update(locationId, name, businessId, capacity, price);
        }

        public void Update(int locationId
                           , string name
                           , int businessId
                           , int capacity
                           , int price)
        {
            Name = name;
            LocationId = locationId;
            BusinessId = businessId;
            Capacity = capacity;
            Price = price;
        }
    }
}