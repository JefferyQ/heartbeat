using HeartBeatPortal.Clients;
using HeartBeatPortal.HeartbeatServer;
using HeartBeatPortal.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeartBeatPortal.Controllers
{
    public class DropDownListController : Controller
    {
        // GET: GetDetails
        public JsonResult GetServers([DataSourceRequest] DataSourceRequest request)
        {
            var client = PortalClients.HeartBeatServerClient;

            var servers = new List<Models.DropDownList>();


            var serversResponse = client.GetServers(new GetServersRequest()
            {
                AllServers = true
            });

            int i = 1;

            foreach (var serverInfo in serversResponse.ServerInfoList)
            {
                servers.Add(new Models.DropDownList { Text = serverInfo.ServerName, Value = i.ToString()});
                ++i;
            }

            return Json(servers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServices([DataSourceRequest] DataSourceRequest request, ServerServiceDetails server)
        {

            var client = PortalClients.HeartBeatServerClient;

            var services = new List<Models.DropDownList>();

            var serviceResponse = client.GetServices(new GetServicesRequest()
            {
                ServerName = server.ServerName,
                AllServices = false
            });

            int i = 1;

            foreach (var item in serviceResponse.ServiceInfoList)
            {
                services.Add(new Models.DropDownList { Text = item.ApplicationNamek__BackingField, Value = i.ToString()});
                ++i;
            }
    

            return Json(services, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMethods(ServerServiceDetails service)
        {

            var client = PortalClients.HeartBeatServerClient;

            var methods = new List<Models.DropDownList>();

            var methodResponse = client.GetMethodsOfService(new GetMethodsOfServiceRequest()
            {
                ServerName = service.ServerName,
                ServiceName = service.ServiceName
            });

            int i = 1;

            foreach (var item in methodResponse.MethodList)
            {
                methods.Add(new Models.DropDownList { Text = item.MethodName, Value = i.ToString()});
                ++i;
            }

            return Json(methods, JsonRequestBehavior.AllowGet);
        }
    }
}