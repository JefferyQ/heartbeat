using HeartbeatServer.dto;
using System.Collections.Generic;

namespace HeartbeatServer.Response
{
    public class GetMethodDurationorCountResponse
    {
        public List<AverageOrDurationDetails> Details { get; set; }
    }
}
