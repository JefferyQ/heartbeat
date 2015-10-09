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
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = Multi.English, Value = "English", Selected = true });
            items.Add(new SelectListItem() { Text = Multi.Turkish, Value = "Turkish", Selected = false });
            ViewBag.Languages = items;
        }
    }
}