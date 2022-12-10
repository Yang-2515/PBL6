using Booking.Domain.Entities;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
                , user.Claims.First(c => c.Type == "businessId").Value
                , user.Claims.First(c => c.Type == "avatar").Value);
        }

        public async Task SendMessageToFirebase(string userId, string username, string title, string notiToUserId)
        {
            /*var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
            });

            Console.WriteLine(defaultApp.Name); // "[DEFAULT]"*/

            if (FirebaseApp.DefaultInstance == null)
            {
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
                });
            }
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    ["Username"] = username,
                    ["UserId"] = userId
                },
                Notification = new Notification
                {
                    Title = title,
                    Body = ""
                },

                //Token = "d3aLewjvTNw:APA91bE94LuGCqCSInwVaPuL1RoqWokeSLtwauyK-r0EmkPNeZmGavSG6ZgYQ4GRjp0NgOI1p-OAKORiNPHZe2IQWz5v1c3mwRE5s5WTv6_Pbhh58rY0yGEMQdDNEtPPZ_kJmqN5CaIc",
                Topic = notiToUserId
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            Console.WriteLine(result);
        }
    }
}
