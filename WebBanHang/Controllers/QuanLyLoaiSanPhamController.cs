using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyLoaiSanPhamController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
        // GET: QuanLyLoaiSanPham
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<LoaiSanPham> lstLoaiSP = dbContext.LoaiSanPhams;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstLoaiSP = lstLoaiSP.Where(x => x.TenLoai.Contains(Keyword));
            }
            return View(lstLoaiSP.OrderBy(x => x.TenLoai).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult ThemMoiLoaiSanPham()
        {
            return View();
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult ThemMoiLoaiSanPham(LoaiSanPham LoaiSanPham)
        {
            if (ModelState.IsValid)
            {
                dbContext.LoaiSanPhams.Add(LoaiSanPham);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult SuaThongTinLoaiSanPham(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham nd = dbContext.LoaiSanPhams.FirstOrDefault(x => x.MaLoaiSP == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            return View(nd);
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult SuaThongTinLoaiSanPham(LoaiSanPham LoaiSanPham)
        {
            dbContext.Entry(LoaiSanPham).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpDelete]
        public ActionResult XoaLoaiSanPham(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham nd = dbContext.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            dbContext.LoaiSanPhams.Remove(nd);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var nd = dbContext.LoaiSanPhams.Find(int.Parse(id));
                dbContext.LoaiSanPhams.Remove(nd);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}