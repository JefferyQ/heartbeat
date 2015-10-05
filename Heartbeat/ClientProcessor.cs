using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace Heartbeat
{
    public class ClientProcessor
    {
        private readonly InfoReserve _infoReserve;
        private Timer _flushTimer;
        private string LocalIp;

        public ClientProcessor()
        {
            _infoReserve = new InfoReserve
            {
                Created = DateTime.Now,
                ExecStats = new List<MethodExecutionInfo>()
            };

            var heartbeatFlushIntervalSeconds = ConfigurationManager.AppSettings["HeartbeatFlushIntervalSeconds"];
            
            int heartbeatIntervalSeconds;
            if (string.IsNullOrEmpty(heartbeatFlushIntervalSeconds) ||
                !int.TryParse(heartbeatFlushIntervalSeconds, out heartbeatIntervalSeconds))
            {
                Console.WriteLine("HeartBeatFlushIntervalSeconds parameter is invalid in Configuration. The value is set to 60 sec.");
                heartbeatIntervalSeconds = 60;
            }

            LocalIp = GetLocalIpAddress();

            _flushTimer = new Timer
            {
                Interval = heartbeatIntervalSeconds * 1000
            };
            _flushTimer.Elapsed += _flushTimer_Elapsed;
            _flushTimer.Start();
        }

        /// <summary>
        /// Send Upd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _flushTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var strReserve = "";
            Monitor.Enter(_infoReserve);
            try
            {
                var appStats = CalculateAppStats();
                strReserve = JsonConvert.SerializeObject(appStats);
                _infoReserve.Created = DateTime.Now;
                _infoReserve.ExecStats.Clear();
            }
            finally
            {
                Monitor.Exit(_infoReserve);
            }
            UdpSender udpSender = new UdpSender();
            udpSender.Send(strReserve);
        }

        public void AddHeartbeatContext(MethodExecutionInfo context)
        {
            var acquiredLock = false;
            Monitor.Enter(_infoReserve, ref acquiredLock);
            if (!acquiredLock)
                Console.WriteLine("NO LOCK!");
            try
            {
                _infoReserve.ExecStats.Add(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (acquiredLock)
                    Monitor.Exit(_infoReserve);
            }
        }

        private AppStats CalculateAppStats()
        {
            var appStats = new AppStats
            {
                Id = Guid.NewGuid(),
                MethodStats = new List<MethodExecutionStats>(),
                EndDate = DateTime.Now,
                StartDate = _infoReserve.Created,
                ClientIp = LocalIp,
                ClientMachine = Environment.MachineName,
                ApplicationName = ConfigurationManager.AppSettings["HeartbeatApplicationName"]
            };

            var methods = _infoReserve.ExecStats
                .Select(m => m.MethodName)
                .Distinct();

            foreach (var method in methods)
            {
                var methodStats = new MethodExecutionStats
                {
                    MethodName = method,
                    ExecutionCount = _infoReserve.ExecStats.Count(m => m.MethodName == method),
                    ExceptionCount = _infoReserve.ExecStats.Where(m=> m.HasException).Count(m => m.MethodName == method),
                    AverageDuration = (int) _infoReserve.ExecStats.Where(m => m.MethodName == method).Average(m => m.Duration),
                    MaxDuration = _infoReserve.ExecStats.Where(m => m.MethodName == method).Max(m => m.Duration),
                    MinDuration = _infoReserve.ExecStats.Where(m => m.MethodName == method).Min(m => m.Duration)
                };
                appStats.MethodStats.Add(methodStats);
            }

            return appStats;
        }

        private static string GetLocalIpAddress()
        {
            IPHostEntry host;
            string localIp = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }
    }
}