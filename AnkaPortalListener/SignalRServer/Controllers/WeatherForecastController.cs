using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace SignalRServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static HubConnection converterConnection;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {   
            if(converterConnection == null)
            {
                converterConnection = new HubConnectionBuilder()
                     .WithUrl("https://localhost:7188/Converter")
                     .WithAutomaticReconnect() //Without any parameters, WithAutomaticReconnect() configures the client to wait 0, 2, 10, and 30 seconds respectively. Can be configured
                     .Build();

                //Receive message from server
                converterConnection.On<string>("MessageReceived", m =>
                {
                    Console.WriteLine(m);
                });

                converterConnection.Reconnecting += error =>
                {
                    // Notify users the connection was lost and the client is reconnecting.
                    // Start queuing or dropping messages.

                    return Task.CompletedTask;
                };

                converterConnection.Reconnected += connectionId =>
                {
                    // Notify users the connection was reestablished.
                    // Start dequeuing messages queued while reconnecting if any.

                    return Task.CompletedTask;
                };

                //If the client doesn't successfully reconnect within its first four attempts, the HubConnection will transition to the Disconnected state and fire the Closed event
                converterConnection.Closed += error =>
                {   
                    //Restart connection manually
                    return Task.CompletedTask;
                };

                await converterConnection.StartAsync();
            }
           

            var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            await converterConnection.InvokeAsync("Receive", converterConnection.ConnectionId, JsonSerializer.Serialize(result));

            return result;


        }

        private void OnReceivedAction(string message)
        {
            
        }
    }
}