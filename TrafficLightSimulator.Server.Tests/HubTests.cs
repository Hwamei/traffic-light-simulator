using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR_UnitTestingSupportCommon.Hubs;
using TrafficLightSimulator.Server.Hubs;
using TrafficLightSimulator.Server.Models;
using TrafficLightSimulator.Server.Services;

namespace TrafficLightSimulator.Server.Tests
{
    public class HubTests
    {
        private readonly Mock<ITrafficLightService> _mockService;

        public HubTests()
        {
            _mockService = new Mock<ITrafficLightService>();
        }

        [Fact]
        public void TestHub_ConnectionId_ExpectedResult()
        {
            var support = new HubUnitTestsSupport();
            support.SetUp();
            var hub = new TrafficLightHub(_mockService.Object);
            support.AssignToHubRequiredProperties(hub);

            var connectionId = hub.Context.ConnectionId;

            Assert.True(!string.IsNullOrWhiteSpace(connectionId));
        }
    }
}
