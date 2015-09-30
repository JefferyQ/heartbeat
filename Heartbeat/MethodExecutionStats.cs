using System;

namespace Heartbeat
{
    [Serializable]
    public class MethodExecutionStats
    {
        /// <summary>
        /// Method Name
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Max Duration
        /// </summary>
        public long MaxDuration { get; set; }
        /// <summary>
        /// Min Duration
        /// </summary>
        public long MinDuration { get; set; }
        /// <summary>
        /// Execution Count
        /// </summary>
        public int ExecutionCount { get; set; }
        /// <summary>
        /// Average Duration
        /// </summary>
        public int AverageDuration { get; set; }
        /// <summary>
        /// Exception Count
        /// </summary>
        public int ExceptionCount { get; set; }
    }
}