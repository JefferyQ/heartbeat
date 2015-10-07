using System.Collections.Generic;
using System.ServiceModel;
using Heartbeat;
using HeartbeatServer.Request;
using HeartbeatServer.Response;

namespace HeartbeatServer
{
    [ServiceContract]
    public interface IHeartbeatServer
    {
        //[OperationContract]
        //SummaryResponse GetSummary();

        //[OperationContract]
        //List<AppStats> GetAllAppStats();

        //[OperationContract]
        //void SaveDisk();

        [OperationContract]
        GetAllServersResponse GetAllServers(GetAllServersRequest request);
    }
}