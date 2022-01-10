using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
        // GET: SanPham
        public ActionResult DanhSachSanPham(int Page = 1, int PageSize = 8, string Order = "")
        {
            ViewBag.Order = Order;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false);
            if (Order == "GiaTangDan" || Order == "")
            {
                return View(lstSanPham.OrderBy(x => x.DonGia).ToPagedList(Page, PageSize));
            }
            else
            {
                return View(lstSanPham.OrderByDescending(x => x.DonGia).ToPagedList(Page, PageSize));
            }
            
        }
        public ActionResult Detail(int id)
        {
            ViewBag.SanPhamBanChay = dbContext.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(x=>x.SoLanMua).Take(3);
            ViewBag.SanPhamKhac = dbContext.SanPhams.Where(n => n.DaXoa == false).Take(4);
            var sp = dbContext.SanPhams.FirstOrDefault(n => n.MaSP == id && n.DaXoa == false);
            return View(sp);
        }
       
    }
}