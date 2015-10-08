using System;

namespace HeartbeatServer.dto
{
    public class AverageOrDurationDetails
    {
        public int ExecutionCount { get; set; }
        public long AverageDuration { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
