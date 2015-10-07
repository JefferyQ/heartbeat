using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartbeatServer.dto
{
    public class MethodLoadDetails
    {

        public string MethodName { get; set; }
        public string ApplicationName { get; set; }
        public string ServerName { get; set; }
        public long Load { get; set; }
        public int ExecutionCount { get; set; }
        public int ExceptionCount { get; set; }
        public long AverageDuration { get; set; }


    }
}
