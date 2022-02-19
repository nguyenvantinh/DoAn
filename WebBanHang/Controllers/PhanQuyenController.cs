using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class PhanQuyenController : Controller
    {
        SellPhoneContext db = new SellPhoneContext();
        // GET: PhanQuyen
        [CustomAuthorize(Roles = "QuanTri")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NguoiDung nd = db.NguoiDungs.SingleOrDefault(n => n.MaNguoiDung == id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            // Lấy danh sách quyền để load ra checkbox
            ViewBag.MaQuyen = db.Quyens;
            //Lấy ra danh sách quyền của nguoi dung do
            ViewBag.NguoiDung_Quyen = db.NguoiDung_Quyen.Where(n => n.MaNguoiDung == id);
            return View(nd);
        }

        [HttpPost]
        public ActionResult Index(int? MaNguoiDung, IEnumerable<NguoiDung_Quyen> lstPhanQuyen)
        {
            //Trường hợp : Nếu đã đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            // Xóa all những quyền đã được gán cho nguoi dung đó
            var lstAsigned = db.NguoiDung_Quyen.Where(n => n.MaNguoiDung == MaNguoiDung);
            if (lstAsigned.Count() != 0)
            {
                db.NguoiDung_Quyen.RemoveRange(lstAsigned);
                db.SaveChanges();
            }
            // it nhat 1 quyen dc check
            if (lstPhanQuyen != null)
            {
                // lặp list quyền được check
                foreach (var item in lstPhanQuyen)
                {
                    item.MaNguoiDung = int.Parse(MaNguoiDung.ToString());
                    db.NguoiDung_Quyen.Add(item);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}