using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<KhachHang> lstKhachHang = dbContext.KhachHangs;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstKhachHang = lstKhachHang.Where(x => x.HoTen.Contains(Keyword) || x.SoDienThoai.Contains(Keyword) || x.Email.Contains(Keyword) || x.DiaChi.Contains(Keyword));
            }
            return View(lstKhachHang.OrderBy(x => x.HoTen).ToPagedList(Page, PageSize));
        }
    }
}