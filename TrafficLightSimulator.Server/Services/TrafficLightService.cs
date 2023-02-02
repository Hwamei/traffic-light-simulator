using Microsoft.AspNetCore.SignalR;
using TrafficLightSimulator.Server.Hubs;
using TrafficLightSimulator.Server.Models;
using System.Text.Json;
using TrafficLightSimulator.Server.Enums;
using Microsoft.Extensions.Options;

namespace TrafficLightSimulator.Server.Services
{
    public class TrafficLightService : ITrafficLightService
    {
        private readonly IHubContext<TrafficLightHub> _hubContext;
        private readonly AppSettings _appSettings;
        public readonly TrafficLightMessage _trafficLightMessage;

        private readonly TimeSpan _normalHoursUpdateInterval;
        private readonly TimeSpan _peakHoursAxisYUpdateInterval;
        private readonly TimeSpan _peakHoursAxisXUpdateInterval;
        private readonly TimeSpan _yellowLightUpdateInterval;
        private readonly TimeSpan _redLightDelayUpdateInterval;
        private readonly TimeSpan _rightTurnUpdateInterval;
        private readonly int _peakHourMorningStart;
        private readonly int _peakHourMorningEnd;
        private readonly int _peakHourEveningStart;
        private readonly int _peakHourEveningEnd;
        private readonly bool _isNorthboundRightTurnEnabled;
        private readonly Timer _timer;

        private readonly object _updateLock = new();
        private volatile bool _updating;

        private bool _isAxisY = true;
        private bool _isNorthboundRightTurnCompleted = false;

        public TrafficLightService(IHubContext<TrafficLightHub> hubContext, IOptions<AppSettings> appSettings)
        {
            _hubContext = hubContext;
            _appSettings = appSettings.Value;
            _trafficLightMessage = new TrafficLightMessage();

            _normalHoursUpdateInterval = TimeSpan.FromSeconds(_appSettings.NormalHoursIntervalInSeconds);
            _peakHoursAxisYUpdateInterval = TimeSpan.FromSeconds(_appSettings.PeakHoursAxisYIntervalInSeconds);
            _peakHoursAxisXUpdateInterval = TimeSpan.FromSeconds(_appSettings.PeakHoursAxisXIntervalInSeconds);
            _yellowLightUpdateInterval = TimeSpan.FromSeconds(_appSettings.YellowLightIntervalInSeconds);
            _redLightDelayUpdateInterval = TimeSpan.FromSeconds(_appSettings.RedLightDelayIntervalInSeconds);
            _rightTurnUpdateInterval = TimeSpan.FromSeconds(_appSettings.RightTurnIntervalInSeconds);
            _peakHourMorningStart = _appSettings.PeakHourMorningStart;
            _peakHourMorningEnd = _appSettings.PeakHourMorningEnd;
            _peakHourEveningStart = _appSettings.PeakHourEveningStart;
            _peakHourEveningEnd = _appSettings.PeakHourEveningEnd;
            _isNorthboundRightTurnEnabled = _appSettings.IsNorthboundRightTurnEnabled;
            var obj = new object();
            _timer = new Timer(Update, obj, _normalHoursUpdateInterval, _normalHoursUpdateInterval);

            if (IsPeakHours())
            {
                var northboundTrafficLight = _trafficLightMessage.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Northbound).Single();

                if (northboundTrafficLight.TrafficLightStatusDict[StatusEnum.Green])
                {
                    SetTimer(_peakHoursAxisYUpdateInterval);
                }

                if (northboundTrafficLight.TrafficLightStatusDict[StatusEnum.Red])
                {
                    SetTimer(_peakHoursAxisXUpdateInterval);
                }
            }
        }

        public async Task SendMessageAsync(string method)
            => await _hubContext.Clients.All.SendAsync(method, JsonSerializer.Serialize(_trafficLightMessage));

        private void SetTimer(TimeSpan interval) 
            => _timer.Change(interval, interval);

        private void Update(object? state)
        {
            try
            {
                lock (_updateLock)
                {
                    if (!_updating)
                    {
                        _updating = true;

                        if (_isAxisY)
                        {
                            var axisYTrafficLights = _trafficLightMessage.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Northbound || tl.Direction == DirectionEnum.Southbound);
                            SetTrafficLights(axisYTrafficLights, _isAxisY);
                        }
                        else
                        {
                            var axisXTrafficLights = _trafficLightMessage.TrafficLights.Where(tl => tl.Direction == DirectionEnum.Eastbound || tl.Direction == DirectionEnum.Westbound);
                            SetTrafficLights(axisXTrafficLights, _isAxisY);

                            _isNorthboundRightTurnCompleted = false;
                        }

                        SendMessageAsync("Changed");
                        _updating = false;
                    }
                }
            }
            catch (Exception)
            {
                Failed();
            }
        }

        private void SetTrafficLights(IEnumerable<TrafficLight> trafficLights, bool isAxisY)
        {
            try
            {
                var isTimerChanged = false;
                foreach (var trafficLight in trafficLights)
                {
                    var southboundAfterNorthboundRightTurn = isAxisY && _isNorthboundRightTurnCompleted && !trafficLight.TrafficLightStatusDict[StatusEnum.Green] && trafficLight.TrafficLightStatusDict[StatusEnum.Red];
                    if (trafficLight.TrafficLightStatusDict[StatusEnum.Green] || southboundAfterNorthboundRightTurn)
                    {
                        if (isAxisY && _isNorthboundRightTurnEnabled && !_isNorthboundRightTurnCompleted)
                        {
                            trafficLight.TrafficLightStatusDict[StatusEnum.RightTurn] = true;
                            var southbound = _trafficLightMessage.TrafficLights.Single(tl => tl.Direction == DirectionEnum.Southbound);
                            southbound.TrafficLightStatusDict[StatusEnum.Green] = false;
                            southbound.TrafficLightStatusDict[StatusEnum.Red] = true;
                            SetTimer(_rightTurnUpdateInterval);
                            _isNorthboundRightTurnCompleted = true;
                            break;
                        }

                        trafficLight.TrafficLightStatusDict[StatusEnum.Green] = false;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Yellow] = isAxisY && trafficLight.TrafficLightStatusDict[StatusEnum.Red] ? false : true;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Red] = isAxisY && trafficLight.TrafficLightStatusDict[StatusEnum.Red] ? true : false;
                        trafficLight.TrafficLightStatusDict[StatusEnum.RightTurn] = false;

                        if (!isTimerChanged)
                        {
                            SetTimer(_yellowLightUpdateInterval);
                            isTimerChanged = true;
                        }
                    }
                    else if (trafficLight.TrafficLightStatusDict[StatusEnum.Yellow])
                    {
                        trafficLight.TrafficLightStatusDict[StatusEnum.Green] = false;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Yellow] = false;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Red] = true;

                        if (!isTimerChanged)
                        {
                            SetTimer(_redLightDelayUpdateInterval);
                            isTimerChanged = true;
                            _isAxisY = !isAxisY;
                        }
                    }
                    else if (trafficLight.TrafficLightStatusDict[StatusEnum.Red])
                    {
                        trafficLight.TrafficLightStatusDict[StatusEnum.Green] = true;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Yellow] = false;
                        trafficLight.TrafficLightStatusDict[StatusEnum.Red] = false;

                        if (!isTimerChanged)
                        {
                            var interval = IsPeakHours() ? (isAxisY ? _peakHoursAxisYUpdateInterval : _peakHoursAxisXUpdateInterval) : _normalHoursUpdateInterval;
                            SetTimer(interval);
                            isTimerChanged = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsPeakHours()
        {
            var timeOfDay = DateTime.Now.TimeOfDay;
            var _isPeakHours = (timeOfDay >= new TimeSpan(_peakHourMorningStart, 0, 0) && timeOfDay <= new TimeSpan(_peakHourMorningEnd, 0, 0)) ||
                (timeOfDay >= new TimeSpan(_peakHourEveningStart, 0, 0) && timeOfDay <= new TimeSpan(_peakHourEveningEnd, 0, 0));
            return _isPeakHours;
        }

        private void Failed()
        {
            try
            {
                foreach (var trafficLight in _trafficLightMessage.TrafficLights)
                {
                    foreach (var key in trafficLight.TrafficLightStatusDict.Keys)
                    {
                        trafficLight.TrafficLightStatusDict[key] = false;
                    }
                }
                SendMessageAsync("Changed");
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                _timer.Dispose();
            }
        }
    }
}
