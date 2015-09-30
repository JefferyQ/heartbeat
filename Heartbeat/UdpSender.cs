using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Heartbeat
{
    public class UdpSender
    {
        public void Send(string strReserve)
        {
            string udpServerIp = ConfigurationManager.AppSettings["HeartbeatUdpServerIP"];
            int udpServerPort = int.Parse(ConfigurationManager.AppSettings["HeartbeatUdpServerPort"]);
            var client = new UdpClient();
            IPAddress address = null;

            if (udpServerIp == "B")
                address = IPAddress.Parse(IPAddress.Broadcast.ToString());
            else
                address = IPAddress.Parse(udpServerIp);

            client.Connect(address, udpServerPort);

            var bytes = Encoding.ASCII.GetBytes(strReserve);
            client.Send(bytes, bytes.GetLength(0));

            client.Close();
        }
    }
}