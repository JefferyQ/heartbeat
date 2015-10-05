using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Heartbeat;
using Newtonsoft.Json;

namespace HeartbeatServer
{
    internal class UdpServer : IDisposable
    {
        private readonly AppStatsProcessor _appStatsProcessor;
        private UdpClient _udpServer;
        public UdpServer(AppStatsProcessor appStatsProcessor)
        {
            _appStatsProcessor = appStatsProcessor;
            Init();
        }

        private void Init()
        {
            int udpServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["UdpServerPort"]);
            _udpServer = new UdpClient(udpServerPort);
            Console.WriteLine(" S E R V E R   IS   S T A R T E D ");
            Console.WriteLine("* Waiting for Clients...");
            _udpServer.BeginReceive(DataReceived, _udpServer);
        }

        private void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;

            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII
            string receivedText = Encoding.ASCII.GetString(receivedBytes);
            AppStats appStats = JsonConvert.DeserializeObject<AppStats>(receivedText);
            _appStatsProcessor.AddNewAppStats(appStats);

            Console.WriteLine("Data Received From : " + appStats.ClientMachine + " / " + appStats.ClientIp + " / " +
                              appStats.ApplicationName);
            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);
        }

        //TODO : Not working :)
        public void Dispose()
        {
            _udpServer.Close();  //close the connection
        }
    }
}