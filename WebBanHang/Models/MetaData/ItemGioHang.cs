//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace WebBanHang.Models
//{
//    [MetadataType(typeof(ItemGioHangMetaData))]
//    public partial class ItemGioHang
//    {
//    }
//    public class ItemGioHangMetaData
//    {
//        public int ID { get; set; }
//        public Nullable<int> MaSP { get; set; }
//        public string TenSP { get; set; }
//        public string HinhAnh { get; set; }
//        public Nullable<int> soluong { get; set; }
//        public Nullable<int> DonGia { get; set; }
//        public Nullable<int> ThanhTien { get; set; }
//        public ItemGioHangMetaData(int maSP)
//        {
//            using (SellPhoneContext db = new SellPhoneContext())
//            {
//                SanPham sp = db.SanPhams.Single(n => n.MaSP == maSP);
//                this.MaSP = maSP;
//                this.TenSP = sp.TenSP;
//                this.HinhAnh = sp.HinhAnh;
//                this.soluong = 1;
//                this.DonGia = (int?)sp.DonGia.Value;
//                this.ThanhTien = DonGia * soluong;

//            }
//        }
//    }
//}