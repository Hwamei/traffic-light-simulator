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

        private readonly TrafficLightService _trafficLightService;
        private readonly TrafficLightMessage _message;
        private readonly Dictionary<StatusEnum, bool> _northbound;
        private readonly Dictionary<StatusEnum, bool> _southbound;
        private readonly Dictionary<StatusEnum, bool> _eastbound;
        private readonly Dictionary<StatusEnum, bool> _westbound;

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

            _trafficLightService = new TrafficLightService(_hub.Object, _mockAppSettings.Object);
            _message = _trafficLightService._trafficLightMessage;

            _northbound = _message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Northbound).Select(tl => tl.TrafficLightStatusDict).Single();
            _southbound = _message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Southbound).Select(tl => tl.TrafficLightStatusDict).Single();
            _eastbound = _message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Eastbound).Select(tl => tl.TrafficLightStatusDict).Single();
            _westbound = _message.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Westbound).Select(tl => tl.TrafficLightStatusDict).Single();
        }

        [Fact]
        public void TestTrafficLightService_Init_ExpectedResult()
        {
            Assert.Equal(4, _message.TrafficLights.Length);

            Assert.True(_northbound[StatusEnum.Green]);
            Assert.False(_northbound[StatusEnum.Yellow]);
            Assert.False(_northbound[StatusEnum.Red]);
            Assert.False(_northbound[StatusEnum.RightTurn]);

            Assert.True(_southbound[StatusEnum.Green]);
            Assert.False(_southbound[StatusEnum.Yellow]);
            Assert.False(_southbound[StatusEnum.Red]);
            Assert.False(_southbound[StatusEnum.RightTurn]);

            Assert.True(_eastbound[StatusEnum.Red]);
            Assert.False(_eastbound[StatusEnum.Yellow]);
            Assert.False(_eastbound[StatusEnum.Green]);
            Assert.False(_eastbound[StatusEnum.RightTurn]);

            Assert.True(_westbound[StatusEnum.Red]);
            Assert.False(_westbound[StatusEnum.Yellow]);
            Assert.False(_westbound[StatusEnum.Green]);
            Assert.False(_westbound[StatusEnum.RightTurn]);
        }

        [Fact]
        public void TestTrafficLightService_Update_Once_ExpectedResult()
        {
            _trafficLightService.Update(null);

            Assert.Equal(4, _message.TrafficLights.Length);

            Assert.True(_northbound[StatusEnum.Yellow]);
            Assert.False(_northbound[StatusEnum.Green]);
            Assert.False(_northbound[StatusEnum.Red]);
            Assert.False(_northbound[StatusEnum.RightTurn]);

            Assert.True(_southbound[StatusEnum.Yellow]);
            Assert.False(_southbound[StatusEnum.Green]);
            Assert.False(_southbound[StatusEnum.Red]);
            Assert.False(_southbound[StatusEnum.RightTurn]);

            Assert.True(_eastbound[StatusEnum.Red]);
            Assert.False(_eastbound[StatusEnum.Yellow]);
            Assert.False(_eastbound[StatusEnum.Green]);
            Assert.False(_eastbound[StatusEnum.RightTurn]);

            Assert.True(_westbound[StatusEnum.Red]);
            Assert.False(_westbound[StatusEnum.Yellow]);
            Assert.False(_westbound[StatusEnum.Green]);
            Assert.False(_westbound[StatusEnum.RightTurn]);
        }

        [Fact]
        public void TestTrafficLightService_Update_Twice_ExpectedResult()
        {
            _trafficLightService.Update(null);
            _trafficLightService.Update(null);

            Assert.Equal(4, _message.TrafficLights.Length);
            Assert.False(_message.TrafficLights.All(tl => tl.TrafficLightStatusDict.All(v => v.Value)));
        }
    }
}