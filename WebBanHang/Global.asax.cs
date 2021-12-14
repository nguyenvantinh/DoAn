using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebBanHang.Controllers;
using WebBanHang.Models;

namespace WebBanHang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        //public void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //    var cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (cookie != null)
        //    {
        //        var authTicket = FormsAuthentication.Decrypt(cookie.Value);
        //        var quyen = authTicket.UserData.Split(new char[] { ',' });
        //var userPrincipal = new GenericPrincipal(new GenericIdentity(cookie.Name), quyen);
        //        Context.User = userPrincipal;
        //    }
        //}

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                // bên AccountController đối tượng FormsAuthenticationTicket authTicket (có thuộc tính UserData,.. ) đã được tạo, mã hóa và làm value của cái Cookie trả về) >> ở đây phải giải mã cái value của cookie đó để lấy lại cái đối tượng FormsAuthenticationTicket authTicket 

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                // userData bên AccountController đã được serialize >> ở đây phải Deserialize về object
                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);
                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);
                principal.UserId = serializeModel.UserId;
                principal.TaiKhoan = serializeModel.TaiKhoan;
                principal.HoTen = serializeModel.HoTen;
                principal.Roles = serializeModel.RoleName.ToArray<string>();

                HttpContext.Current.User = principal;
            }
        }
        protected void Application_BeginRequest()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }
    }
}
