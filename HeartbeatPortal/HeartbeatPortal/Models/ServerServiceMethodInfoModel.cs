using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartBeatPortal.Models
{
    public class ServerServiceMethodInfoModel
    {

        public ServerServiceMethodInfoModel()
        {
            ServerName = new List<string>();
            ServiceName = new List<string>();
            MethodName = new List<string>();
        }

        public List<string> ServerName { get; set; }
        public List<string> ServiceName { get; set; }
        public List<string> MethodName { get; set; }   

    }
}