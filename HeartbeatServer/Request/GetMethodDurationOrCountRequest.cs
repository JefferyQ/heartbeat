﻿

namespace HeartbeatServer.Request
{
    public class GetMethodDurationOrCountRequest
    {
        public string MethodName { get; set; }
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public int MethodNumber { get; set; }
        public string DataType { get; set; }
    }
}
