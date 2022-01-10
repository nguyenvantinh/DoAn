using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        SellPhoneContext db = new SellPhoneContext();
        // GET: QuanLyPhieuNhap
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {

            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            // SaveChanges lần đầu để  sinh ra mã phiếu nhập gán cho lstChiTietPhieuNhap
            SanPham sp;
            foreach (var item in lstModel)
            {
                // Cập nhật số lượng tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                // Gán mã phiếu nhập cho từng bản ghi của chi tiết phiếu nhập, những thuộc tính khác của ctpn đã dc binding tương ứng từ view
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            // đk số lượng tồn < = 5 .. tình trạng đã xóa false
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);
        }

        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }
        // Xử lý nhập hàng từng sản phẩm
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            model.DaXoa = false;
            model.NgayNhap = DateTime.Now;
            db.PhieuNhaps.Add(model);
            // Save lấy mã phiếu nhập
            db.SaveChanges();
            ctpn.MaPN = model.MaPN;
            // Cập nhật tồn
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return RedirectToAction("NhapHangDon", sp.MaSP);
        }


        // Giải phóng vùng nhớ
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                }
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}