
namespace HeartbeatServer.Request
{
    public class GetTopMethodLoadRequest
    {

        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public int MethodNumber { get; set; }

    }
}
