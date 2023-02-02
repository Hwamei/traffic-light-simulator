namespace TrafficLightSimulator.Server.Services
{
    public interface ITrafficLightService
    {
        Task SendMessageAsync(string method);
    }
}
