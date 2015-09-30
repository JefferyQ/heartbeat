using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Heartbeat;
using Timer = System.Timers.Timer;


namespace HeartbeatServer
{
    internal class AppStatsProcessor
    {
        private readonly HbArchiveProcessor _hbArchiveProcessor;
        public AppStatsProcessor(HbArchiveProcessor hbArchiveProcessor)
        {
            _hbArchiveProcessor = hbArchiveProcessor;
        }

        internal void AddNewAppStats(AppStats tmpAppStats)
        {
           _hbArchiveProcessor.AddAppStats(tmpAppStats);
        }
        /*
        internal string[] GetServers()
        {
            return _appStats.Select(m => new {server = m.ClientMachine + " / " + m.ClientIp}).Distinct().Select(source => source.ToString().Replace("server = ", "")).ToArray();
        }

        internal int GetAppStatsCount()
        {
            return _appStats.ExecutionCount;
        }

        internal List<AppStats> GetAllAppStats()
        {
            return _appStats;
        }
        */
    }
}