using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebBanHang.Controllers
{
    public class CustomPrincipal : IPrincipal
    {
        public int UserId { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}