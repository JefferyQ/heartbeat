using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartBeatPortal.Models
{
    public class ServiceDiag
    {
        public string ServiceName { get; set; }
        public string ServerName { get; set; }
        public DateTime FirstHeartbeat { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
}