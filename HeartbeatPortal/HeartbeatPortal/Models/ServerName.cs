using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartBeatPortal.Models
{
    public class ServerName
    {
        public string Name { get; set; }
        public DateTime LastHb { get; set; }
    }
}