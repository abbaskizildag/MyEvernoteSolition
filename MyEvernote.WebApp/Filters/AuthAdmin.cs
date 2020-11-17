using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User!= null && CurrentSession.User.IsAdmin==false)
            {
                filterContext.Result = new RedirectResult("/Home/AccesDenied");
                //burada busayfaya yetkiniz yok dediğimiz bir sayfa
            }
        }
    }
}