using System;

namespace HeartbeatServer
{
    [Serializable]
    public class ServiceInfo
    {
        /// <summary>
        /// Server Name related with ApplicationName
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Service Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Received last HeartBeatDate
        /// </summary>
        public DateTime LastHeartBeatDate { get; set; }
    }
}