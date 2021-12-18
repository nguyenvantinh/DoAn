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
        public ActionResult XemGioHang()
        {
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
        public decimal TinhTongTien()
        {
            var listGH = Session["GioHang"] as List<ItemGioHang>;
            if (listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.ThanhTien);
        }
    }
}