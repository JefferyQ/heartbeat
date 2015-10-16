using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HeartBeatPortal.Clients;
using HeartBeatPortal.HeartbeatServer;
using HeartBeatPortal.Models;
using HeartBeatPortal.MultiResources;

namespace HeartBeatPortal.Controllers
{
    public class GetDetailsController : BaseController
    {
        // GET: GetDetails
        public ActionResult Servers(string server)
        {

            var client = PortalClients.HeartBeatServerClient;

            var serverInfo = new ServerDiag();

            serverInfo.ServerName = server;

            var serverResponse = client.GetOneServer(new GetOneServerRequest()
            {
                Name = server
            });

            serverInfo.FirstHeartbeat = serverResponse.FirstHb;
            serverInfo.LastHeartbeat = serverResponse.LastHb;

            return View(serverInfo);

        }

        public ActionResult Services(string server, string service)
        {
            var client = PortalClients.HeartBeatServerClient;

            var serviceInfo = new ServiceDiag();

            serviceInfo.ServiceName = service;
            serviceInfo.ServerName = server;

            var serviceResponse = client.GetOneService(new GetOneServiceRequest()
            {
                ServerName = server,
                ServiceName = service
            });

            serviceInfo.FirstHeartbeat = serviceResponse.FirstHb;
            serviceInfo.LastHeartbeat = serviceResponse.LastHb;

            return View(serviceInfo);

        }

        public ActionResult Methods(string server, string service, string method)
        {
            var client = PortalClients.HeartBeatServerClient;

            var methodInfo = new MethodDiag();

            methodInfo.ServerName = server;
            methodInfo.ServiceName = service;
            methodInfo.MethodName = method;

            var methodResponse = client.GetMethodDetails(new GetMethodDetailRequest()
            {
                ServerName = server,
                ServiceName = service,
                MethodName = method
            });

            methodInfo.ExceptionCount = methodResponse.Details.TotalExceptionCount;
            methodInfo.ExecutionCount = methodResponse.Details.TotalExecutionCount;
            methodInfo.AverageDuration = methodResponse.Details.OverallAverageDuration;
            methodInfo.FirstExecution = methodResponse.Details.FirstExecution;
            methodInfo.LastExecution = methodResponse.Details.LastExecution;
            methodInfo.MaxDuration = methodResponse.Details.MaxDuration;
            methodInfo.MinDuration = methodResponse.Details.MinDuration;



            return View(methodInfo);
        }
    }
}