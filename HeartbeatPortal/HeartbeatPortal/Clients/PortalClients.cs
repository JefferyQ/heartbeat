
using System.Configuration;
namespace HeartBeatPortal.Clients
{
    public class PortalClients
    {
        public static HeartbeatServer.HeartbeatServerClient HeartBeatServerClient = GetHeartBeatServerService();

        public static HeartbeatServer.HeartbeatServerClient GetHeartBeatServerService()
        {
            return new HeartbeatServer.HeartbeatServerClient(ConfigurationManager.AppSettings["IHeartbeatServerBindingName"]);
        }
    }
}