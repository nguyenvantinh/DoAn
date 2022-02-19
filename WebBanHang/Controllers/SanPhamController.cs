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
            if (Order == "GiaGiamDan")
            {
                return View(lstSanPham.OrderByDescending(x => x.DonGia).ToPagedList(Page, PageSize));
            }
            else
            {
                return View(lstSanPham.OrderBy(x => x.DonGia).ToPagedList(Page, PageSize));
            }
        }

        // GetByLoaiSP
        public ActionResult DanhSach(int Page = 1, int PageSize = 8, string Type = "", string Order = "")
        {
            ViewBag.Order = Order;
            ViewBag.Type = Type;
            IEnumerable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false && x.LoaiSanPham.BiDanh == Type).ToList();
            if (Order == "GiaGiamDan")
            {
                return View(lstSanPham.OrderByDescending(x => x.DonGia).ToPagedList(Page, PageSize));
            }
            else
            {
                return View(lstSanPham.OrderBy(x => x.DonGia).ToPagedList(Page, PageSize));
            }
        }

        // GetByThuongHieu
        public ActionResult ThuongHieu(int Page = 1, int PageSize = 8, string t = "", string Order = "")
        {
            ViewBag.Order = Order;
            ViewBag.t = t;
            IEnumerable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false && x.NhaSanXuat.TenNSX == t).ToList();
            if (Order == "GiaGiamDan")
            {
                return View(lstSanPham.OrderByDescending(x => x.DonGia).ToPagedList(Page, PageSize));
            }
            else
            {
                return View(lstSanPham.OrderBy(x => x.DonGia).ToPagedList(Page, PageSize));
            }
        }

        public ActionResult Detail(int id)
        {
            ViewBag.SanPhamBanChay = dbContext.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(x=>x.SoLanMua).Take(3);
            ViewBag.SanPhamKhac = dbContext.SanPhams.Where(n => n.DaXoa == false).Take(4);
            var sp = dbContext.SanPhams.FirstOrDefault(n => n.MaSP == id && n.DaXoa == false);
            return View(sp);
        }

        public ActionResult TimKiem(int Page = 1, int PageSize = 8, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword) && x.DaXoa == false);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }
    }
}