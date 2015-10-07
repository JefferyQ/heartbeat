using System.Collections.Generic;
using Heartbeat;
using HeartbeatServer.Request;
using HeartbeatServer.Response;

namespace HeartbeatServer
{
    class HeartbeatServer : IHeartbeatServer
    {
        private readonly HbArchiveProcessor _hbArchiveProcessor;
        public HeartbeatServer(HbArchiveProcessor hbArchiveProcessor)
        {
            _hbArchiveProcessor = hbArchiveProcessor;
        }

        public GetServersResponse GetServers(GetServersRequest request)
        {
            return _hbArchiveProcessor.GetServers(request);
        }

        public GetServicesResponse GetServices(GetServicesRequest request)
        {
            return _hbArchiveProcessor.GetServices(request);
        }
        


        //public SummaryResponse GetSummary()
        //{
        //    return new SummaryResponse
        //    {
        //        //Servers = _appStatsProcessor.GetServers(),
        //        //TotalAppStats = _appStatsProcessor.GetAppStatsCount()
        //    };
        //}

        //public List<AppStats> GetAllAppStats()
        //{
        //    return null;
        //    //return _appStatsProcessor.GetAllAppStats();
        //}

        //public void SaveDisk()
        //{
        //    //BinarySerialization.WriteToBinaryFile("C:/Users/cozkoc/Desktop/heartbeat_service/heartbeat/Dump.hb", GetAllAppStats());
        //}
    }
}
