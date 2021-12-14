using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.DAL
{
    public class NhaSanXuatDA
    {
        SellPhoneContext db = null;
        public NhaSanXuatDA()
        {
             db = new SellPhoneContext();
        }
        public List<NhaSanXuat> GetDanhSachNSX()
        {
            var lstNSX = db.NhaSanXuats.ToList();
            return lstNSX;
        }
    }
}