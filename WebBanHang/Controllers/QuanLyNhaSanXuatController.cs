using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyNhaSanXuatController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<NhaSanXuat> lstNhaSanXuat = dbContext.NhaSanXuats;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstNhaSanXuat = lstNhaSanXuat.Where(x => x.TenNSX.Contains(Keyword) || x.ThongTin.Contains(Keyword));
            }
            return View(lstNhaSanXuat.OrderBy(x => x.TenNSX).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ThemMoiNhaSanXuat()
        {
            return View();
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiNhaSanXuat(NhaSanXuat nsx, HttpPostedFileBase Logo)
        {
            int loi = 0;
                if (Logo != null)
                {
                    if (Logo.ContentLength > 0)
                    {
                        if (Logo.ContentType != "image/jpg" && Logo.ContentType != "image/jpeg" && Logo.ContentType != "image/png")
                        {
                            ViewBag.LoiDinhDang += "Hình ảnh không đúng định dạng <br>";
                            loi++;
                        }
                        else
                        {
                            // check file exist
                            var FileName = Path.GetFileName(Logo.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), FileName);
                            // if exist
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.LocTrung += "Hình ảnh đã tồn tại <br>";
                                loi++;
                            }
                        }
                    }
                }
            if (loi > 0) // any error image then return view
            {
                return View(nsx);
            }
            if (ModelState.IsValid)
            {
                if (Logo != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), Logo.FileName);
                    Logo.SaveAs(path);
                    nsx.Logo = Logo.FileName;
                }
                dbContext.NhaSanXuats.Add(nsx);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(nsx);
            }
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult SuaThongTinNhaSanXuat(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nsx = dbContext.NhaSanXuats.FirstOrDefault(x => x.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }
            Session["Logo"] = nsx.Logo;
            return View(nsx);
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult SuaThongTinNhaSanXuat(NhaSanXuat nsx, HttpPostedFileBase Logo)
        {
            int loi = 0;
            if (Logo != null)
            {
                if (Logo.ContentLength > 0)
                {
                    if (Logo.ContentType != "image/jpg" && Logo.ContentType != "image/jpeg" && Logo.ContentType != "image/png")
                    {
                        ViewBag.LoiDinhDang += "Hình ảnh không đúng định dạng <br>";
                        loi++;
                    }
                    else
                    {
                        // check file exist
                        var FileName = Path.GetFileName(Logo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), FileName);
                        // if exist
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.LocTrung += "Hình ảnh đã tồn tại <br>";
                            loi++;
                        }
                    }
                }
            }
            if (loi > 0) // any error image then return view
            {
                return View(nsx);
            }
            
            if (ModelState.IsValid)
            {
                if (Logo != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), Logo.FileName);
                    Logo.SaveAs(path);
                    nsx.Logo = Logo.FileName;
                }
                else
                {
                    nsx.Logo = Session["Logo"].ToString();
                }
                dbContext.Entry(nsx).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(nsx);
            }
        }

        [CustomAuthorize(Roles = "QuanTri")]
        [HttpDelete]
        public ActionResult XoaNhaSanXuat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nsx = dbContext.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }
            dbContext.NhaSanXuats.Remove(nsx);
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
                var nsx = dbContext.NhaSanXuats.Find(int.Parse(id));
                dbContext.NhaSanXuats.Remove(nsx);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}