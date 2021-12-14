using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;
            ViewResult viewResult = null;
            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Admin",
                        action = "Login",
                    }
                    ));
                filterContext.Result = routeData;
            }
            else
            {
                //routeData = new RedirectToRouteResult
                //(new System.Web.Routing.RouteValueDictionary
                // (new
                // {
                //     controller = "Error",
                //     action = "AccessDenied"
                // }
                // ));
                viewResult = new ViewResult
                {
                    ViewName = "~/Views/Admin/AccessDenied.cshtml"
                };
                filterContext.Result = viewResult;
            }
        }
    }
}
