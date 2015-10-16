using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HeartBeatPortal.Models;
using HeartBeatPortal.Clients;
using HeartBeatPortal.HeartbeatServer;
using Kendo.Mvc.Extensions;

namespace HeartBeatPortal.Controllers
{
    public class GridReadingController : BaseController
    {
        // GET: GridReading
        public ActionResult ServiceNameBinding([DataSourceRequest] DataSourceRequest request, ServerName server)
        {

            var client = PortalClients.HeartBeatServerClient;

            var list = new List<ServiceName>();

            var serviceResponse = client.GetServices(new GetServicesRequest()
            {
                ServerName = server.Name
            });

            foreach (var service in serviceResponse.ServiceInfoList)
            {
                list.Add(new ServiceName {Name = service.ApplicationNamek__BackingField});
            }

            return Json(list.ToDataSourceResult(request));
        }

        public ActionResult TopMethodBinding([DataSourceRequest] DataSourceRequest request, ServerName server)
        {

            var client = PortalClients.HeartBeatServerClient;

            var topMethods = new List<TopMethodLoads>();

            var topMethodResponse = client.GetTopMethods(new GetTopMethodLoadRequest()
            {
                ServerName = server.Name,
                MethodNumber = 50
            });

            foreach (var method in topMethodResponse.MethodLoadDetailList)
            {
                topMethods.Add(new TopMethodLoads {ApplicationName = method.ApplicationName, MethodName = method.MethodName, Load = method.Load, ExceptionCount = method.ExceptionCount, ExecutionCount = method.ExecutionCount, AverageDuration = method.AverageDuration});
            }


            return Json(topMethods.ToDataSourceResult(request));
        }

        public ActionResult ServerNameBinding([DataSourceRequest] DataSourceRequest request, ServiceName service)
        {
            var client = PortalClients.HeartBeatServerClient;

            var serverInfo = new List<ServerName>();

            var serverResponse = client.GetServers(new GetServersRequest()
            {
                ServiceName = service.Name
            });

            foreach (var item in serverResponse.ServerInfoList)
            {
                serverInfo.Add(new ServerName {Name = item.ServerName, LastHb = item.LastHb});
            }

            return Json(serverInfo.ToDataSourceResult(request));
        }

        public ActionResult MethodNameBinding([DataSourceRequest] DataSourceRequest request, ServiceName service)
        {

            var client = PortalClients.HeartBeatServerClient;

            var names = new List<MethodName>();

            var methodResponse = client.GetAllMethods(new GetAllMethodsRequest()
            {
                ServerName = service.ServerName,
                ServiceName = service.Name

            });

            foreach (var item in methodResponse.MethodNameList)
            {
                names.Add(new MethodName {Name = item});
            }

            return Json(names.ToDataSourceResult(request));
        }

        public ActionResult TopDurationBinding([DataSourceRequest] DataSourceRequest request, ServerServiceDetails details)
        {

            var client = PortalClients.HeartBeatServerClient;

            var list = new List<Top10DurationOrCount>();

            var responseDuration = client.GetTopMethodDurationOrCount(new GetMethodDurationOrCountRequest()
            {
                ServerName = details.ServerName,
                ServiceName = details.ServiceName,
                MethodName = details.MethodName,
                MethodNumber = 10,
                DataType = DataTypes.Average
            });

            foreach (var item in responseDuration.Details)
            {
                list.Add(new Top10DurationOrCount {Duration = item.AverageDuration, ExecutionTime = item.ExecutionTime});
            }

            return Json(list.ToDataSourceResult(request));
        }

        public ActionResult TopExecutionBinding([DataSourceRequest] DataSourceRequest request, ServerServiceDetails details)
        {

            var client = PortalClients.HeartBeatServerClient;

            var list = new List<Top10DurationOrCount>();

            var responseDuration = client.GetTopMethodDurationOrCount(new GetMethodDurationOrCountRequest()
            {
                ServerName = details.ServerName,
                ServiceName = details.ServiceName,
                MethodName = details.MethodName,
                MethodNumber = 10,
                DataType = DataTypes.ExecutionCount
            });

            foreach (var item in responseDuration.Details)
            {
                list.Add(new Top10DurationOrCount { ExeCount = item.ExecutionCount, ExecutionTime = item.ExecutionTime });
            }

            return Json(list.ToDataSourceResult(request));
        }
    }
}