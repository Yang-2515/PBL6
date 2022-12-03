using Microsoft.AspNetCore.SignalR;

namespace Booking.API.Controllers
{
    public class SignalHub : Hub
    {
        public async Task NewMessage(string user, string message)
        {
            await Clients.All.SendAsync("messageReceived", user, message);
        }
    }
}
