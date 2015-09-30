using System;
using System.Collections.Generic;

namespace Heartbeat
{
    [Serializable]
    public class AppStats
    {
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
        ///     Method Stats
        /// </summary>
        public List<MethodExecutionStats> MethodStats { get; set; }

        /// <summary>
        ///     Guid
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Stats Start Date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Stats End Date
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}