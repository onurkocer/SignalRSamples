using ConverterListener;
using Microsoft.AspNetCore.SignalR;

namespace SignalRListenerSample
{
    public class ConverterListener : Hub
    {
        public async Task<string> Receive(string connectionId, string message)
        {   
            //Response to client whose sends the current message
            await Clients.Client(connectionId).SendAsync("MessageReceived", "message");

            //Response to caller whose sends the current message
            await Clients.Caller.SendAsync("MessageReceived", "message2");
       
            //Can send message with establish new connection
            //AnkaPortalMessageSender.SendMessage("Convert completed");

            return "test";
        }
    }
}
