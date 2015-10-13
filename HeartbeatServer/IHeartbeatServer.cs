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
        [OperationContract]
        GetServersResponse GetServers(GetServersRequest request);

        [OperationContract]
        GetServicesResponse GetServices(GetServicesRequest request);

        [OperationContract]
        GetTopMethodLoadResponse GetTopMethods(GetTopMethodLoadRequest request);

        [OperationContract]
        GetMethodDetailResponse GetMethodDetails(GetMethodDetailRequest request);

        [OperationContract]
        GetMethodDurationorCountResponse GetTopMethodDurationOrCount(GetMethodDurationOrCountRequest request);

        [OperationContract]
        GetMethodsOfServiceResponse GetMethodsOfService(GetMethodsOfServiceRequest request);

        [OperationContract]
        GetAllMethodsResponse GetAllMethods(GetAllMethodsRequest request);
    }
}