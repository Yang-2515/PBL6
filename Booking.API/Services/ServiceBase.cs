using Booking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Services
{
    public class ServiceBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ServiceBase(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public User GetCurrentUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            return new User(user.Claims.First(c => c.Type == "id").Value
                , user.Claims.First(c => c.Type == "firstName").Value
                , user.Claims.First(c => c.Type == "businessId").Value);
        }
    }
}
