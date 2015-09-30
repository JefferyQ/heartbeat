using System;

namespace Heartbeat
{
    public class MethodExecutionInfo
    {
        /// <summary>
        /// Method Name
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Trigger Date
        /// </summary>
        public DateTime ExecutionDate { get; set; }
        /// <summary>
        /// Duration
        /// </summary>
        public long Duration { get; set; }
        /// <summary>
        /// Has Exception
        /// </summary>
        public bool HasException { get; set; }
    }
}