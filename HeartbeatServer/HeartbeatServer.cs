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

        public GetAllServersResponse GetAllServers(GetAllServersRequest request)
        {
            return _hbArchiveProcessor.AllServers(request);
        }

        public GetTopMethodLoadResponse GetTopMethods(GetTopMethodLoadRequest request)
        {
            return _hbArchiveProcessor.TopNMethods(request);
        }

        public GetMethodDetailResponse GetMethodDetails(GetMethodDetailRequest request)
        {
            return _hbArchiveProcessor.MethodDetails(request);
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
