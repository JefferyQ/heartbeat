using System;

namespace HeartbeatServer
{
    [Serializable]
    public class HbTempArchiveItem
    {
        /// <summary>
        /// Date of Temp Archive Last Insert
        /// </summary>
        public DateTime StatDate { get; set; }

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
        ///     ExecutionCount
        /// </summary>
        public int ExecutionCount { get; set; }

        /// <summary>
        ///     Average Duration
        /// </summary>
        public int AverageDuration { get; set; }

        /// <summary>
        ///     ExceptionCount 
        /// </summary>
        public int ExceptionCount { get; set; } // exception count yoktu, eklendi
    }
}