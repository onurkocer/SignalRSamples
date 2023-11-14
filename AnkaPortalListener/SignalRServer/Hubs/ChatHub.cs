using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string name, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", name, message);
        }
    }
}
