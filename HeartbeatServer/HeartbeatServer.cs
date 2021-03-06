﻿using System.Collections.Generic;
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

        public GetTopMethodLoadResponse GetTopMethods(GetTopMethodLoadRequest request)
        {
            return _hbArchiveProcessor.TopNMethods(request);
        }

        public GetMethodDetailResponse GetMethodDetails(GetMethodDetailRequest request)
        {
            return _hbArchiveProcessor.MethodDetails(request);
        }

        public GetMethodDurationorCountResponse GetTopMethodDurationOrCount(GetMethodDurationOrCountRequest request)
        {
            return _hbArchiveProcessor.DurationOrCount(request);
        }

        public GetMethodsOfServiceResponse GetMethodsOfService(GetMethodsOfServiceRequest request)
        {
            return _hbArchiveProcessor.GetMethodsOfService(request);
        }

        public GetAllMethodsResponse GetAllMethods(GetAllMethodsRequest request)
        {
            return _hbArchiveProcessor.GetMethods(request);
        }

        public GetOneServerResponse GetOneServer(GetOneServerRequest request)
        {
            return _hbArchiveProcessor.GetOneServerAndHb(request);
        }

        public GetOneServiceResponse GetOneService(GetOneServiceRequest request)
        {
            return _hbArchiveProcessor.GetOneServiceAndHb(request);
        }

    }
}
