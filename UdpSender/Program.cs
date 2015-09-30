using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpSender
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            //IPAddress address = IPAddress.Parse(IPAddress.Broadcast.ToString());
            IPAddress address = IPAddress.Parse(IPAddress.Broadcast.ToString());
            client.Connect(address, 8008);
            while (true)
            {
                var bytes = Encoding.ASCII.GetBytes("New Message From : " + LocalIPAddress());
                client.Send(bytes, bytes.GetLength(0));
                Thread.Sleep(1000);
            }
            client.Close();
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
