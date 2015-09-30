using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Heartbeat;
using Newtonsoft.Json;

namespace UdpReceiver
{
    internal class Program
    {
        private static List<AppStats> _appStats = null;
        private static void Main(string[] args)
        {
            _appStats = new List<AppStats>();

            string data = "";
            UdpClient server = new UdpClient(8008);
            Console.WriteLine(" S E R V E R   IS   S T A R T E D ");
            Console.WriteLine("* Waiting for Client...");

            server.BeginReceive(DataReceived, server);

            Console.WriteLine("Press Enter To Finish");
            Console.ReadLine(); //delay end of program
            server.Close();  //close the connection
        }

        private static void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            string receivedText = Encoding.ASCII.GetString(receivedBytes);
            //AppStats appStats = JsonConvert.DeserializeObject<AppStats>(receivedText);
            //_appStats.Add(appStats);

            //Console.WriteLine("AppStat Count : " + _appStats.Count);
            //Console.WriteLine("AppStat Exec Count : " + _appStats.Sum(m=> m.ms.Sum(x=> x.cnt)));

            Console.WriteLine(receivedText);

            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);
        }
    }
}
