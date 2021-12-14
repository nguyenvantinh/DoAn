using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class CustomRole : RoleProvider
    {
        //Notice that GetRolesForUser method accepts username as parameter, then it will return all roles of the given username.For IsUserInRole method takes username and rolename as parameters and checks if user has a role that will allow him access to the requested resource.
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (SellPhoneContext dbContext = new SellPhoneContext())
            {
                var selectedUser = (from us in dbContext.NguoiDungs.Include("NguoiDung_Quyen")
                                    where string.Compare(us.TaiKhoan, username, StringComparison.OrdinalIgnoreCase) == 0
                                    select us).FirstOrDefault();
                if (selectedUser != null)
                {
                    userRoles = new[] { selectedUser.NguoiDung_Quyen.Select(r => r.MaQuyen).ToString() };
                }

                return userRoles.ToArray();
            }
        }

        #region others
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }



        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}