using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.DAL
{

    public class NhaCungCapDA
    {
        SellPhoneContext db = null;
        public NhaCungCapDA()
        {
             db = new SellPhoneContext();
        }
        public List<NhaCungCap> GetDanhSachNCC()
        {
            var lstNCC = db.NhaCungCaps.ToList();
            return lstNCC;
        }
    }
}