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
    public class QuanLyQuyenController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpGet]
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<Quyen> lstQuyen = dbContext.Quyens;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstQuyen = lstQuyen.Where(x => x.MaQuyen.Contains(Keyword) || x.TenQuyen.Contains(Keyword));
            }
            return View(lstQuyen.OrderBy(x => x.MaQuyen).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpGet]
        public ActionResult ThemMoiQuyen()
        {
            return View();
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpPost]
        public ActionResult ThemMoiQuyen(Quyen Quyen)
        {
            if (ModelState.IsValid)
            {
                dbContext.Quyens.Add(Quyen);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpGet]
        public ActionResult SuaThongTinQuyen(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quyen nd = dbContext.Quyens.FirstOrDefault(x => x.MaQuyen == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            return View(nd);
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpPost]
        public ActionResult SuaThongTinQuyen(Quyen Quyen)
        {
            dbContext.Entry(Quyen).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpDelete]
        public ActionResult XoaQuyen(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quyen nd = dbContext.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            dbContext.Quyens.Remove(nd);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLQuyen")]
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var nd = dbContext.Quyens.Find(int.Parse(id));
                dbContext.Quyens.Remove(nd);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}