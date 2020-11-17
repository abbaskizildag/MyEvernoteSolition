using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Filters
{
    public class Exc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["LastError"] = filterContext.Exception; //hatayı aktardık.
            filterContext.ExceptionHandled = true; //hatayı ben yöneticem demej
            filterContext.Result = new RedirectResult("/Home/HasError");
        }
    }
}