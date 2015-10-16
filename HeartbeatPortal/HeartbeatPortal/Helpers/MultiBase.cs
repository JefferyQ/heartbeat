using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HeartBeatPortal.Helpers
{
    public class MultiBase : Controller
    {
        protected override void ExecuteCore()
        {
            string CultureName = null;
            string Language = null;
            HttpCookie cultureCookie = Request.Cookies["Lang"];

            if (cultureCookie != null)
            {

                Language = cultureCookie.Value;

                switch (Language)
                {

                    case "English":
                        CultureName = "en-us";
                        break;
                    case "Turkish":
                        CultureName = "tr";
                        break;
                    default:
                        CultureName = "en-us";
                        break;
                }
            }

            else
            {

                CultureName = "en-us";

            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            base.ExecuteCore();

        }

        protected override bool DisableAsyncSupport
        {
            get
            {
                return true;
            }
        }
    }
}