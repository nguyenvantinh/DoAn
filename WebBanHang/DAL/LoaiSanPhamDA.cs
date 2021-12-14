using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.DAL
{
    public class LoaiSanPhamDA
    {
        SellPhoneContext db = null;
        public LoaiSanPhamDA()
        {
            db = new SellPhoneContext();
        }
        public List<LoaiSanPham> GetDanhSachLSP()
        {
            var lstLSP = db.LoaiSanPhams.ToList();
            return lstLSP;
        }
    }
}