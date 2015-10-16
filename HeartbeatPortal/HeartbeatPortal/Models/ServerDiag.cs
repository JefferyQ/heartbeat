using System;
using System.Collections.Generic;

namespace HeartBeatPortal.Models
{
    public class ServerDiag
    {
        public string ServerName { get; set; }
        public DateTime FirstHeartbeat { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
}