using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Moq;
using TrafficLightSimulator.Server.Enums;
using TrafficLightSimulator.Server.Hubs;
using TrafficLightSimulator.Server.Models;
using TrafficLightSimulator.Server.Services;

namespace TrafficLightSimulator.Server.Tests
{
    public class TrafficLightServiceTests
    {
        private readonly Mock<IHubContext<TrafficLightHub>> _hub;
        private readonly Mock<IOptions<AppSettings>> _mockAppSettings;

        private readonly Mock<IClientProxy> _mockClientProxy = new();
        private readonly Mock<IHubClients> _mockClients = new();
        private readonly Mock<HubCallerContext> _mockContext = new();

        public TrafficLightServiceTests()
        {
            _hub = new Mock<IHubContext<TrafficLightHub>>();
            _mockClients.Setup(client => client.All).Returns(_mockClientProxy.Object);
            _mockContext.Setup(context => context.ConnectionId).Returns(It.IsIn<string>("connection-id"));
            _hub.Setup(hub => hub.Clients).Returns(_mockClients.Object);

            _mockAppSettings = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings 
            {
                NormalHoursIntervalInSeconds = 20,
                PeakHoursAxisYIntervalInSeconds = 40,
                PeakHoursAxisXIntervalInSeconds = 10,
                YellowLightIntervalInSeconds = 5,
                RedLightDelayIntervalInSeconds = 4,
                RightTurnIntervalInSeconds = 10,
                PeakHourMorningStart = 8,
                PeakHourMorningEnd = 10,
                PeakHourEveningStart = 17,
                PeakHourEveningEnd = 19,
                IsNorthboundRightTurnEnabled = false
            };
            _mockAppSettings.Setup(ap => ap.Value).Returns(appSettings);
        }

        [Fact]
        public void TestTrafficLightService_Init_ExpectedResult()
        {
            var trafficLightService = new TrafficLightService(_hub.Object, _mockAppSettings.Object);

            var message = trafficLightService._trafficLightMessage;

            Assert.Equal(4, message.TrafficLights.Length);

            var northbound = message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Northbound).Select(tl => tl.TrafficLightStatusDict).Single();
            Assert.True(northbound[StatusEnum.Green]);
            Assert.False(northbound[StatusEnum.Yellow]);
            Assert.False(northbound[StatusEnum.Red]);
            Assert.False(northbound[StatusEnum.RightTurn]);

            var southbound = message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Southbound).Select(tl => tl.TrafficLightStatusDict).Single();
            Assert.True(southbound[StatusEnum.Green]);
            Assert.False(southbound[StatusEnum.Yellow]);
            Assert.False(southbound[StatusEnum.Red]);
            Assert.False(southbound[StatusEnum.RightTurn]);

            var eastbound = message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Eastbound).Select(tl => tl.TrafficLightStatusDict).Single();
            Assert.True(eastbound[StatusEnum.Red]);
            Assert.False(eastbound[StatusEnum.Yellow]);
            Assert.False(eastbound[StatusEnum.Green]);
            Assert.False(eastbound[StatusEnum.RightTurn]);

            var westbound = message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Westbound).Select(tl => tl.TrafficLightStatusDict).Single();
            Assert.True(westbound[StatusEnum.Red]);
            Assert.False(westbound[StatusEnum.Yellow]);
            Assert.False(westbound[StatusEnum.Green]);
            Assert.False(westbound[StatusEnum.RightTurn]);
        }
    }
}