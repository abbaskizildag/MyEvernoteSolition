using MyEvernote.Common;
using MyEvernote.WebApp.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyEvernote.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            App.common = new WebCommon(); //burada bunu demeseydil common'daki defaultcommon ile çalýþýcaktý. þimdi webcommon ile çalýþ diyoruz.
        }
    }
}
