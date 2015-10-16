
using System;

namespace HeartbeatServer.dto
{
    public class MethodDetails
    {
        public string MethodName { get; set; }
        public string ServerName { get; set; }
        public string ApplicationName { get; set; }
        public long OverallAverageDuration { get; set; }
        public int TotalExecutionCount { get; set; }
        public int TotalExceptionCount { get; set; }
        public DateTime FirstExecution { get; set; }
        public DateTime LastExecution { get; set; }
        public long MaxDuration { get; set; }
        public long MinDuration { get; set; }

    }
}
