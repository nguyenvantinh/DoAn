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
            var listKH = from kh in db.KhachHangs select kh;

            return View(listKH);
        }

        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> listGH = Session["GioHang"] as List<ItemGioHang>;
            if(listGH == null)
            {
                listGH = new List<ItemGioHang>();
                Session["GioHang"] = listGH;
            }
            return listGH;
        }

        public ActionResult ThemItemGioHang(int maSP)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == maSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var listGH = LayGioHang();
            ItemGioHang spCheck = listGH.SingleOrDefault(n => n.MaSP == maSP);
            if(spCheck != null)
            {
                if(sp.SoLuongTon < spCheck.soluong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
                }
                //cập nhật số lượng của sp đó
                spCheck.soluong++;
                //cập nhật tổng tiền của sp đó
                spCheck.ThanhTien = spCheck.soluong * spCheck.DonGia;
                //ViewBag.SoLuongItemGioHang = TinhSoLuongItemGioHang();
                Session["CartItems"] = TinhSoLuongItemGioHang();
                return PartialView("GioHangPartial");
            }
            ItemGioHang newItem = new ItemGioHang(maSP);
            if (sp.SoLuongTon < newItem.soluong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
            }
            listGH.Add(newItem);
            //ViewBag.SoLuongItemGioHang = TinhSoLuongItemGioHang();
            Session["CartItems"] = TinhSoLuongItemGioHang();
            return PartialView("GioHangPartial");
        }

        public int TinhSoLuongItemGioHang()
        {
            var listGH = Session["GioHang"] as List<ItemGioHang>;
            if(listGH == null)
            {
                return 0;
            }
            return listGH.Sum(n => n.soluong);
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

        public ActionResult GioHangPartial()
        {
            if(TinhSoLuongItemGioHang() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhSoLuongItemGioHang();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
    }
}