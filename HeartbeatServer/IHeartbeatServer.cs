using System.Collections.Generic;
using System.ServiceModel;
using Heartbeat;

namespace HeartbeatServer
{
    [ServiceContract]
    public interface IHeartbeatServer
    {
        [OperationContract]
        SummaryResponse GetSummary();

        [OperationContract]
        List<AppStats> GetAllAppStats();

        [OperationContract]
        void SaveDisk();
    }
}