using Microsoft.AspNetCore.SignalR;
using TrafficLightSimulator.Server.Services;

namespace TrafficLightSimulator.Server.Hubs
{
    public class TrafficLightHub : Hub
    {
        private readonly ITrafficLightService _trafficLightService;

        public TrafficLightHub(ITrafficLightService trafficLightService) 
            => _trafficLightService = trafficLightService;

        public override async Task OnConnectedAsync()
        {
            await _trafficLightService.SendMessageAsync("Changed");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception) 
            => await base.OnDisconnectedAsync(exception);
    }
}
