using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeartBeatPortal.Controllers
{
    public class GetDetailsController : BaseController
    {
        // GET: GetDetails
        public ActionResult Servers(string server)
        {
            return View();
        }

        public ActionResult Services(string server, string service)
        {
            return View();
        }

        public ActionResult Methods(string server, string service, string method)
        {
            return View();
        }
    }
}