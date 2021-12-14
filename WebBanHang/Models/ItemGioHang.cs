using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public int soluong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public ItemGioHang(int maSP)
        {
            using (SellPhoneContext db = new SellPhoneContext())
            {
                SanPham sp = db.SanPhams.Single(n => n.MaSP == maSP);
                this.MaSP = maSP;
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.soluong = 1;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * soluong;

            }
        }
    }
}