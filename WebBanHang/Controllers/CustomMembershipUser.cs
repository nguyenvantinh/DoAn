using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class CustomMembershipUser: MembershipUser
    {
        #region User Properties
        public int UserId { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public ICollection<NguoiDung_Quyen> Roles { get; set; }
        #endregion

        public CustomMembershipUser(NguoiDung user) : base("CustomMembership", user.TaiKhoan, user.MaNguoiDung, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.MaNguoiDung;
            TaiKhoan = user.TaiKhoan;
            HoTen = user.HoTen;
            Roles = user.NguoiDung_Quyen;
        }
    }
}