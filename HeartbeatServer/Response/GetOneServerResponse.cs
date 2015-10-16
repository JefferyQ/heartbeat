using System;

namespace HeartbeatServer.Response
{
    public class GetOneServerResponse
    {
        public string ServerName;
        public DateTime FirstHb { get; set; }
        public DateTime LastHb { get; set; }
    }
}
