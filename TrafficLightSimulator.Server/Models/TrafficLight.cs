using TrafficLightSimulator.Server.Enums;

namespace TrafficLightSimulator.Server.Models
{
    public class TrafficLight
    {
        public DirectionEnum Direction { get; set; }
        public Dictionary<StatusEnum, bool> TrafficLightStatusDict { get; set; } = new Dictionary<StatusEnum, bool>();
    }
}
