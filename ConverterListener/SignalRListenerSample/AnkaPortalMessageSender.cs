using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace ConverterListener
{
    public static class AnkaPortalMessageSender
    {
        private static HubConnection ankaPortalConnection;
        public async static void SendMessage(string message)
        {
            if (ankaPortalConnection == null)
            {
                ankaPortalConnection = new HubConnectionBuilder()
                     .WithUrl("https://localhost:7157/converter")
                     .WithAutomaticReconnect()
                     .Build();

                await ankaPortalConnection.StartAsync();
            }

            await ankaPortalConnection.InvokeAsync("Receive", "Username", message);
        }
    }
}
