using System.ServiceModel;
using Heartbeat;

namespace XService
{
    [ServiceContract]
    public interface IXService
    {
        [OperationContract]
        void DoX();

        [OperationContract]
        void DoY();
    }
}