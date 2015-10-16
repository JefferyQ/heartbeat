using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartbeatServer.Response
{
    public class GetOneServiceResponse
    {
        public string ServiceName { get; set; }
        public DateTime FirstHb { get; set; }
        public DateTime LastHb { get; set; }
    }
}
