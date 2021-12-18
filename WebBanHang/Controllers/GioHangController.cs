using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        SellPhoneContext db = new SellPhoneContext();
        public ActionResult XemGioHang(bool isDatHang = false)
        {
            if (isDatHang)
            {
                ViewBag.DatHang = 1;
            }
            var lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                listGH = new List<ItemGioHang>();
                Session["GioHang"] = listGH;
            }
            return listGH;
        }

        [HttpPost]
        public ActionResult ThemItemGioHang(int maSP)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == maSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var listGH = LayGioHang();
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);
            if (spCheck != null)
            {
                if (sp.SoLuongTon <= spCheck.soluong)
                {
                    return Json(new { status = false, mes = "Sản phẩm đã hết hàng" });
                }
                spCheck.soluong++;
                spCheck.ThanhTien = spCheck.soluong * spCheck.DonGia;
                return Json(new { status = true });
            }
            ItemGioHang newItem = new ItemGioHang(maSP);
            if (sp.SoLuongTon < newItem.soluong)
            {
                return Json(new { status = false, mes = "Sản phẩm đã hết hàng" });
            }
            listGH.Add(newItem);
            return Json(new { status = true });
        }
        [HttpPost]
        public ActionResult SuaGioHang(int maSP, int soluong)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == maSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var listGH = LayGioHang();
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);
            if (sp.SoLuongTon <= soluong)
            {
                return Json(new { status = false, mes = "Sản phẩm đã hết hàng" });
            }
            spCheck.soluong = soluong ;
            spCheck.ThanhTien = spCheck.soluong * spCheck.DonGia;
            return Json(new { status = true });
        }

        [HttpPost]
        public ActionResult XoaItemGioHang(int? maSP)
        {
            var listGH = LayGioHang();
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);
            listGH.Remove(spCheck);
            return Json(new { status = true });
        }

        [HttpPost]
        public ActionResult DatHang(KhachHang kh)
        {
            KhachHang khachhang = new KhachHang();
            if (Session["khachhang"] == null)
            {
                khachhang = kh;
                db.KhachHangs.Add(khachhang);
                db.SaveChanges();
            }
            else
            {
                khachhang = Session["khachhang"] as KhachHang;
            }
            //thêm đơn hàng
            DonDatHang dondathang = new DonDatHang();
            dondathang.MaKhachHang = khachhang.MaKhachHang;
            dondathang.NgayDat = DateTime.Now;
            dondathang.TinhTrangGiaoHang = false;
            dondathang.UuDai = 0;
            dondathang.DaHuy = false;
            dondathang.DaThanhToan = false;
            dondathang.DaXoa = false;
            db.DonDatHangs.Add(dondathang);
            db.SaveChanges();

            // thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            foreach (var item in lstGioHang)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = dondathang.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.soluong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang", new { isDatHang = true});
        }
        public ActionResult TinhSoLuongItemGioHang()
        {
            var listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                return Json(new { status = false, Total = 0 }, JsonRequestBehavior.AllowGet);
            }
            var sum = listGH.Sum(n => n.soluong);
            return Json(new { status = true, Total = sum }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TinhTongTien()
        {
            var listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                return Json(new { status = false, TongTien = 0 }, JsonRequestBehavior.AllowGet);
            }
           var sum = listGH.Sum(n => n.ThanhTien);
            return Json(new { status = true, TongTien = sum }, JsonRequestBehavior.AllowGet);
        }
    }
}