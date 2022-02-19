using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (Session["KhachHang"] != null)
            {
                if (isDatHang)
                {
                    ViewBag.DatHang = 1;
                }
                var lstGioHang = LayGioHang();
                return View(lstGioHang);
            }
            return RedirectToAction("Login", "KhachHang");
        }

        public List<ItemGioHang> LayGioHang()//lấy dữ liệu từ session giohang ép kiểu về list itemgiohang, now: get all item from itemgiohang table
        {
            //if (Session["KhachHang"] != null)
            //{
                var kh = (KhachHang)Session["KhachHang"];
                List<ItemGioHang> listGH = db.ItemGioHangs.Where(n => n.MaKH == kh.MaKhachHang).ToList();
                if (listGH == null)
                {
                    listGH = new List<ItemGioHang>();
                }
                return listGH;
            //}
            //return new List<ItemGioHang>();
        }

        [HttpPost]
        public ActionResult ThemItemGioHang(int maSP)
        {
            if (Session["KhachHang"] != null)
            {
                var kh = (KhachHang)Session["KhachHang"];
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
                    db.Entry(spCheck).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { status = true });
                }
                ItemGioHang newItem = new ItemGioHang(maSP);
                if (sp.SoLuongTon < newItem.soluong)
                {
                    return Json(new { status = false, mes = "Sản phẩm đã hết hàng" });
                }
                newItem.MaKH = kh.MaKhachHang;
                listGH.Add(newItem);
                db.ItemGioHangs.Add(newItem);
                db.SaveChanges();
                return Json(new { status = true });
            }
            return Json(new { status = 2});
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
            db.Entry(spCheck).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { status = true });
        }

        [HttpPost]
        public ActionResult XoaItemGioHang(int? maSP)
        {
            var listGH = LayGioHang();
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);
            listGH.Remove(spCheck);
            db.ItemGioHangs.Remove(spCheck);
            db.SaveChanges();
            return Json(new { status = true });
        }

        [HttpPost]
        public ActionResult DatHang(KhachHang kh)
        {
            KhachHang khachhang = new KhachHang();
            if (Session["KhachHang"] == null)
            {
                //khachhang = kh;
                //db.KhachHangs.Add(khachhang);
                //db.SaveChanges();
                return RedirectToAction("Login", "KhachHang");
            }
            else
            {
                khachhang = Session["KhachHang"] as KhachHang;
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
            ClearGioHang(khachhang.MaKhachHang);
            return RedirectToAction("XemGioHang", new { isDatHang = true});
        }
        public void ClearGioHang(int MaKhachHang)
        {
            var lstSanPham = db.ItemGioHangs.Where(n => n.MaKH == MaKhachHang);
            db.ItemGioHangs.RemoveRange(lstSanPham);
            db.SaveChanges();
        }
        public ActionResult TinhSoLuongItemGioHang()
        {
            if (Session["KhachHang"] != null)
            {
                var listGH = LayGioHang();
                if (listGH == null)
                {
                    return Json(new { status = false, Total = 0 }, JsonRequestBehavior.AllowGet);
                }
                var sum = listGH.Sum(n => n.soluong);
                return Json(new { status = true, Total = sum }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, Total = 0 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TinhTongTien()
        {
            if (Session["KhachHang"] != null)
            {
                var listGH = LayGioHang();
                if (listGH == null)
                {
                    return Json(new { status = false, TongTien = 0 }, JsonRequestBehavior.AllowGet);
                }
                var sum = listGH.Sum(n => n.ThanhTien);
                return Json(new { status = true, TongTien = sum }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, TongTien = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}