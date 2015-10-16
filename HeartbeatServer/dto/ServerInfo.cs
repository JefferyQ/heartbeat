using System;

namespace HeartbeatServer.dto
{
    public class ServerInfo
    {
        public string ServerName { get; set; }
        public DateTime LastHb { get; set; }
    }
}
