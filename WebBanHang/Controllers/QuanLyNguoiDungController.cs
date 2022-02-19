using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyNguoiDungController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpGet]
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<NguoiDung> lstNguoiDung = dbContext.NguoiDungs;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstNguoiDung = lstNguoiDung.Where(x => x.HoTen.Contains(Keyword) || x.DiaChi.Contains(Keyword) || x.Email.Contains(Keyword) || x.SoDienThoai.Contains(Keyword));
            }
            return View(lstNguoiDung.OrderBy(x => x.HoTen).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpGet]
        public ActionResult ThemMoiNguoiDung()
        {
            return View();
        }
        
        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpPost]
        public ActionResult ThemMoiNguoiDung( NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                dbContext.NguoiDungs.Add(nguoiDung);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpGet]
        public ActionResult SuaThongTinNguoiDung( int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nd = dbContext.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            return View(nd);
        }
        
        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpPost]
        public ActionResult SuaThongTinNguoiDung(NguoiDung nguoiDung)
        {
                dbContext.Entry(nguoiDung).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpDelete]
        public ActionResult XoaNguoiDung(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nd = dbContext.NguoiDungs.SingleOrDefault(n => n.MaNguoiDung == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            dbContext.NguoiDungs.Remove(nd);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "QLNguoiDung, QuanTri")]
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var nd = dbContext.NguoiDungs.Find(int.Parse(id));
                dbContext.NguoiDungs.Remove(nd);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}