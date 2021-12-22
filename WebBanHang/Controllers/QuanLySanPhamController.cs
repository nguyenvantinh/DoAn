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
    public class QuanLySanPhamController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
        // GET: QuanLySanPham
        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult Index(int Page = 1, int PageSize = 10, string Keyword="")
        {
            ViewBag.Keyword = Keyword;
            //ViewBag.hienthi = hienthi;
            //if (hienthi != 0)
            //{
            //    PageSize = hienthi;
            //}
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword));
                lstSanPham = lstSanPham.Where(x => x.DaXoa == false);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult ThemMoiSanPham()
        {
            PopulateDropdownlist();
            return View();
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiSanPham(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            int loi = 0;
            for (var i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpg" && HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png")
                        {
                            ViewBag.LoiDinhDang += "Hình ảnh " + i + " không đúng định dạng <br>";
                            loi++;
                        }
                        else
                        {
                            // check file exist
                            var FileName = Path.GetFileName(HinhAnh[i].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), FileName);
                            // if exist
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.LocTrung += "Hình ảnh " + i + " đã tồn tại <br>";
                                loi++;
                            }
                        }
                    }
                }
            }
            if (loi > 0) // any erro image then return view
            {
                PopulateDropdownlist();
                return View(sp);
            }
            if (ModelState.IsValid)
            {
                if (HinhAnh[0] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[0].FileName);
                    HinhAnh[0].SaveAs(path);
                    sp.HinhAnh = HinhAnh[0].FileName;
                }
                else
                {
                    sp.HinhAnh = "Alternate-image.png";
                }
                if (HinhAnh[1] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[1].FileName);
                    HinhAnh[1].SaveAs(path);
                    sp.HinhAnh1 = HinhAnh[1].FileName;
                }
                else
                {
                    sp.HinhAnh1 = "Alternate-image.png";
                }
                if (HinhAnh[2] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[2].FileName);
                    HinhAnh[2].SaveAs(path);
                    sp.HinhAnh2 = HinhAnh[2].FileName;
                }
                else
                {
                    sp.HinhAnh2 = "Alternate-image.png";
                }
                if (HinhAnh[3] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[3].FileName);
                    HinhAnh[3].SaveAs(path);
                    sp.HinhAnh3 = HinhAnh[3].FileName;
                }
                else
                {
                    sp.HinhAnh3 = "Alternate-image.png";
                }
                if (HinhAnh[4] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[4].FileName);
                    HinhAnh[4].SaveAs(path);
                    sp.HinhAnh4 = HinhAnh[4].FileName;
                }
                else
                {
                    sp.HinhAnh4 = "Alternate-image.png";
                }
                dbContext.SanPhams.Add(sp);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                PopulateDropdownlist();
                return View(sp);
            }
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult SuaSanPham(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = dbContext.SanPhams.FirstOrDefault(x => x.MaSP == id && x.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            Session["HinhAnh"] = sp.HinhAnh;
            Session["HinhAnh1"] = sp.HinhAnh1;
            Session["HinhAnh2"] = sp.HinhAnh2;
            Session["HinhAnh3"] = sp.HinhAnh3;
            Session["HinhAnh4"] = sp.HinhAnh4;
            PopulateDropdownlist(sp.MaNSX, sp.MaNCC, sp.MaLoaiSP);
            return View(sp);
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            int loi = 0;
            for (var i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpg" && HinhAnh[i].ContentType != "image/png")
                        {
                            ViewBag.LoiDinhDang += "Hình ảnh " + i + " không đúng định dạng <br>";
                            loi++;
                        }
                        else
                        {
                            // check file exist
                            var FileName = Path.GetFileName(HinhAnh[i].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), FileName);
                            // if exist
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.LocTrung += "Hình ảnh " + i + " đã tồn tại <br>";
                                loi++;
                            }
                        }
                    }
                    //else size invalid
                }
            }
            if (loi > 0) // any erro image then return view
            {
                PopulateDropdownlist();
                return View(sp);

                //return Json(new { success = false, message = ViewBag.LocTrung }, JsonRequestBehavior.AllowGet);

            }
            else // all images are true >> asign images
            {

                if (HinhAnh[0] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[0].FileName);
                    HinhAnh[0].SaveAs(path);
                    sp.HinhAnh = HinhAnh[0].FileName;
                    //var oldPath = Request.MapPath("~/Content/HinhAnhSP/" + Session["HinhAnh"].ToString());
                    //if (System.IO.File.Exists(oldPath))
                    //{
                    //    System.IO.File.Delete(oldPath);
                    //}
                }
                else
                {
                    sp.HinhAnh = Session["HinhAnh"].ToString();
                }
                if (HinhAnh[1] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[1].FileName);
                    HinhAnh[1].SaveAs(path);
                    sp.HinhAnh1 = HinhAnh[1].FileName;
                    //var oldPath = Request.MapPath("~/Content/HinhAnhSP/" + Session["HinhAnh1"].ToString());
                    //if (System.IO.File.Exists(oldPath))
                    //{
                    //    System.IO.File.Delete(oldPath);
                    //}
                }
                else
                {
                    sp.HinhAnh1 = Session["HinhAnh1"].ToString();
                }
                if (HinhAnh[2] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[2].FileName);
                    HinhAnh[2].SaveAs(path);
                    sp.HinhAnh2 = HinhAnh[2].FileName;
                    //var oldPath = Request.MapPath("~/Content/HinhAnhSP/" + Session["HinhAnh2"].ToString());
                    //if (System.IO.File.Exists(oldPath))
                    //{
                    //    System.IO.File.Delete(oldPath);
                    //}
                }
                else
                {
                    sp.HinhAnh2 = Session["HinhAnh2"].ToString();
                }
                if (HinhAnh[3] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[3].FileName);
                    HinhAnh[3].SaveAs(path);
                    sp.HinhAnh3 = HinhAnh[3].FileName;
                    //var oldPath = Request.MapPath("~/Content/HinhAnhSP/" + Session["HinhAnh3"].ToString());
                    //if (System.IO.File.Exists(oldPath))
                    //{
                    //    System.IO.File.Delete(oldPath);
                    //}
                }
                else
                {
                    sp.HinhAnh3 = Session["HinhAnh3"].ToString();
                }
                if (HinhAnh[4] != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), HinhAnh[4].FileName);
                    HinhAnh[4].SaveAs(path);
                    sp.HinhAnh4 = HinhAnh[4].FileName;
                    //var oldPath = Request.MapPath("~/Content/HinhAnhSP/" + Session["HinhAnh4"].ToString());
                    //if (System.IO.File.Exists(oldPath))
                    //{
                    //    System.IO.File.Delete(oldPath);
                    //}
                }
                else
                {
                    sp.HinhAnh4 = Session["HinhAnh4"].ToString();
                }
            }
            if (ModelState.IsValid)
            {
                dbContext.Entry(sp).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
                //return Json(new { success = true, message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PopulateDropdownlist();
                return View(sp);
            }
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpDelete]
        public ActionResult XoaSanPham(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = dbContext.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            sp.DaXoa = true;
            dbContext.SanPhams.Attach(sp);
            dbContext.Entry(sp).Property(x => x.DaXoa).IsModified = true;
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
                var sp = dbContext.SanPhams.Find(int.Parse(id));
                sp.DaXoa = true;
                dbContext.SanPhams.Attach(sp);
                dbContext.Entry(sp).Property(x => x.DaXoa).IsModified = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpDelete]
        public ActionResult XoaSanPhamNoiBat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = dbContext.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            sp.SPNoiBat = false;
            dbContext.SanPhams.Attach(sp);
            dbContext.Entry(sp).Property(x => x.SPNoiBat).IsModified = true;
            dbContext.SaveChanges();
            return RedirectToAction("SanPhamNoiBat");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult DeleteMultiSPNB(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var sp = dbContext.SanPhams.Find(int.Parse(id));
                sp.SPNoiBat = false;
                dbContext.SanPhams.Attach(sp);
                dbContext.Entry(sp).Property(x => x.SPNoiBat).IsModified = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("SanPhamNoiBat");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = dbContext.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        public void PopulateDropdownlist(object selectedNSX = null, object selectedNCC = null, object selectedLSP = null)
        {
            ViewBag.MaNSX = new SelectList(dbContext.NhaSanXuats.OrderBy(x => x.TenNSX), "MaNSX", "TenNSX", selectedNSX);
            ViewBag.MaNCC = new SelectList(dbContext.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", selectedNCC);
            ViewBag.MaLoaiSP = new SelectList(dbContext.LoaiSanPhams.OrderBy(x => x.TenLoai), "MaLoaiSP", "TenLoai", selectedLSP);
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult SanPhamNoiBat(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false && x.SPNoiBat == true);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword));
                lstSanPham = lstSanPham.Where(x => x.DaXoa == false && x.SPNoiBat == true);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult ThemMoiSanPhamNoiBat(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var sp = dbContext.SanPhams.Find(int.Parse(id));
                sp.SPNoiBat = true;
                dbContext.SanPhams.Attach(sp);
                dbContext.Entry(sp).Property(x => x.SPNoiBat).IsModified = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("SanPhamNoiBat");
        }
        
        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpGet]
        public ActionResult SanPhamMoi(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false && x.Moi == 1);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword));
                lstSanPham = lstSanPham.Where(x => x.DaXoa == false && x.Moi == 1);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult ThemSanPhamMoi(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var sp = dbContext.SanPhams.Find(int.Parse(id));
                sp.Moi = 1;
                dbContext.SanPhams.Attach(sp);
                dbContext.Entry(sp).Property(x => x.Moi).IsModified = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("SanPhamMoi");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpDelete]
        public ActionResult XoaSanPhamMoi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = dbContext.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            sp.Moi = 0;
            dbContext.SanPhams.Attach(sp);
            dbContext.Entry(sp).Property(x => x.Moi).IsModified = true;
            dbContext.SaveChanges();
            return RedirectToAction("SanPhamMoi");
        }

        [CustomAuthorize(Roles = "QLSanPham")]
        [HttpPost]
        public ActionResult DeleteMultiSPMoi(FormCollection formCollection)
        {
            string[] lstID = formCollection["itemID"].Split(new char[] { ',' });
            foreach (var id in lstID)
            {
                var sp = dbContext.SanPhams.Find(int.Parse(id));
                sp.Moi = 0;
                dbContext.SanPhams.Attach(sp);
                dbContext.Entry(sp).Property(x => x.Moi).IsModified = true;
                dbContext.SaveChanges();
            }
            return RedirectToAction("SanPhamMoi");
        }
        public ActionResult DanhSachSP(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword));
                lstSanPham = lstSanPham.Where(x => x.DaXoa == false);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }
        public ActionResult DanhSachSanPham(int Page = 1, int PageSize = 10, string Keyword = "")
        {
            ViewBag.Keyword = Keyword;
            IQueryable<SanPham> lstSanPham = dbContext.SanPhams.Where(x => x.DaXoa == false);
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword) || x.NhaSanXuat.TenNSX.Contains(Keyword) || x.NhaCungCap.TenNCC.Contains(Keyword));
                lstSanPham = lstSanPham.Where(x => x.DaXoa == false);
            }
            return View(lstSanPham.OrderBy(x => x.TenSP).ToPagedList(Page, PageSize));
        }

    }
}