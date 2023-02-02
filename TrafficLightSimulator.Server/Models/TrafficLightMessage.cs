using TrafficLightSimulator.Server.Enums;

namespace TrafficLightSimulator.Server.Models
{
    public class TrafficLightMessage
    {
        public TrafficLight[] TrafficLights { get; set; } = new TrafficLight[4]
        {
            new TrafficLight
            {
                Direction = DirectionEnum.Northbound,
                TrafficLightStatusDict = new Dictionary<StatusEnum, bool>
                {
                    { StatusEnum.Red, false },
                    { StatusEnum.Yellow, false },
                    { StatusEnum.Green, true },
                    { StatusEnum.RightTurn, false }
                }
            },
            new TrafficLight
            {
                Direction = DirectionEnum.Eastbound,
                TrafficLightStatusDict = new Dictionary<StatusEnum, bool>
                {
                    { StatusEnum.Red, true },
                    { StatusEnum.Yellow, false },
                    { StatusEnum.Green, false },
                    { StatusEnum.RightTurn, false }
                }
            },
            new TrafficLight
            {
                Direction = DirectionEnum.Southbound,
                TrafficLightStatusDict = new Dictionary<StatusEnum, bool>
                {
                    { StatusEnum.Red, false },
                    { StatusEnum.Yellow, false },
                    { StatusEnum.Green, true },
                    { StatusEnum.RightTurn, false }
                }
            },
            new TrafficLight
            {
                Direction = DirectionEnum.Westbound,
                TrafficLightStatusDict = new Dictionary<StatusEnum, bool>
                {
                    { StatusEnum.Red, true },
                    { StatusEnum.Yellow, false },
                    { StatusEnum.Green, false },
                    { StatusEnum.RightTurn, false }
                }
            },
        };
    }
}
