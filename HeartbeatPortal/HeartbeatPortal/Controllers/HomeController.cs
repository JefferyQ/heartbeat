using HeartBeatPortal.Clients;
using HeartBeatPortal.HeartbeatServer;
using HeartBeatPortal.MultiResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeartBeatPortal.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }   
    }
}