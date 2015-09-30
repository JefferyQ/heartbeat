using System;

namespace HeartbeatServer
{
    public class HbArchiveItem
    {
        /// <summary>
        /// yyyyMMdd format int
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 24 Hour (HH) format, hour
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        ///     Client IP Address
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        ///     Client Machine Name
        /// </summary>
        public string ClientMachine { get; set; }

        /// <summary>
        ///     Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     Method Name
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        ///     Max Duration
        /// </summary>
        public long MaxDuration { get; set; }

        /// <summary>
        ///     Min Duration
        /// </summary>
        public long MinDuration { get; set; }

        /// <summary>
        ///     Execution Count
        /// </summary>
        public int ExecutionCount { get; set; }

        /// <summary>
        ///     Average Duration
        /// </summary>
        public int AverageDuration { get; set; }
    }
}