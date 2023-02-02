using Moq;
using System.Dynamic;
using TrafficLightSimulator.Server.Hubs;
using TrafficLightSimulator.Server.Services;

namespace TrafficLightSimulator.Server.Tests
{
    public class TrafficLightServiceTests
    {
        private readonly Mock<ITrafficLightService> _mockService;

        public TrafficLightServiceTests()
        {
            _mockService = new Mock<ITrafficLightService>();
        }
    }
}