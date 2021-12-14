using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public List<string> RoleName { get; set; }
    }
}