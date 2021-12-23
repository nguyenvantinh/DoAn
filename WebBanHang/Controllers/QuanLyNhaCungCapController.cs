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
    public class QuanLyNhaCungCapController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<NhaCungCap> lstNhaCungCap = dbContext.NhaCungCaps;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstNhaCungCap = lstNhaCungCap.Where(x => x.TenNCC.Contains(Keyword) || x.DiaChi.Contains(Keyword) || x.Email.Contains(Keyword) || x.SoDienThoai.Contains(Keyword));
            }
            return View(lstNhaCungCap.OrderBy(x => x.TenNCC).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ThemMoiNhaCungCap()
        {
            return View();
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult ThemMoiNhaCungCap(NhaCungCap NhaCungCap)
        {
            if (ModelState.IsValid)
            {
                dbContext.NhaCungCaps.Add(NhaCungCap);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult SuaThongTinNhaCungCap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nd = dbContext.NhaCungCaps.FirstOrDefault(x => x.MaNCC == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            return View(nd);
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult SuaThongTinNhaCungCap(NhaCungCap NhaCungCap)
        {
            dbContext.Entry(NhaCungCap).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpDelete]
        public ActionResult XoaNhaCungCap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nd = dbContext.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            dbContext.NhaCungCaps.Remove(nd);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var nd = dbContext.NhaCungCaps.Find(int.Parse(id));
                dbContext.NhaCungCaps.Remove(nd);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}