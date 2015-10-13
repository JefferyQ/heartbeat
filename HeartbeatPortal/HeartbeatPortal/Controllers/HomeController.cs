using HeartBeatPortal.Clients;
using HeartBeatPortal.HeartbeatServer;
using HeartBeatPortal.MultiResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HeartBeatPortal.Models;

namespace HeartBeatPortal.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            return View();
        }

        //public ActionResult KendoMenu()
        //{
        //    var client = PortalClients.HeartBeatServerClient;

        //    var details = new ServerServiceMethodInfoModel();


        //    var serversResponse = client.GetServers(new GetServersRequest()
        //    {
        //        AllServers = true
        //    });

        //    foreach (var serverInfo in serversResponse.ServerInfoList)
        //    {
        //        details.ServerName.Add(serverInfo.ServerName);
        //    }

        //    var serviceResponse = client.GetServices(new GetServicesRequest()
        //    {
        //        AllServices = true
        //    });

        //    foreach (var serviceInfo in serviceResponse.ServiceInfoList)
        //    {
        //        details.ServiceName.Add(serviceInfo.ApplicationNamek__BackingField);
        //    }

        //    var methodResponse = client.GetAllMethods(new GetAllMethodsRequest());

        //    foreach (var item in methodResponse.MethodNameList)
        //    {
        //        details.MethodName.Add(item);
        //    }

        //    return View(details);
        //}

    }
}