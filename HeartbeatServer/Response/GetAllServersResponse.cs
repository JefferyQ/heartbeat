
using HeartbeatServer.dto;
using System.Collections.Generic;

namespace HeartbeatServer.Response
{
    public class GetAllServersResponse
    {
        public List<ServerInfo> ServerInfoList { get; set; } 
    }
}
