using Microsoft.AspNetCore.SignalR;

namespace AnkaPortalListener.Hubs
{
    public class ConverterHub : Hub
    {
        public async Task Receive(string name, string message)
        {
           
        }
    }
}
