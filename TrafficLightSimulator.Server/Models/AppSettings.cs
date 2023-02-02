namespace TrafficLightSimulator.Server.Models
{
    public class AppSettings
    {
        public int NormalHoursIntervalInSeconds { get; set; }
        public int PeakHoursAxisYIntervalInSeconds { get; set; }
        public int PeakHoursAxisXIntervalInSeconds { get; set; }
        public int YellowLightIntervalInSeconds { get; set; }
        public int RedLightDelayIntervalInSeconds { get; set; }
        public int RightTurnIntervalInSeconds { get; set; }
        public int PeakHourMorningStart { get; set; }
        public int PeakHourMorningEnd { get; set; }
        public int PeakHourEveningStart { get; set; }
        public int PeakHourEveningEnd { get; set; }
        public bool IsNorthboundRightTurnEnabled { get; set; }
    }
}
