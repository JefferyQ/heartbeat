using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartBeatPortal.Models
{
    public class TopMethodLoads
    {
        public string MethodName { get; set; }
        public string ApplicationName { get; set; }
        public long Load { get; set; }
        public int ExecutionCount { get; set; }
        public int ExceptionCount { get; set; }
        public long AverageDuration { get; set; }
    }
}