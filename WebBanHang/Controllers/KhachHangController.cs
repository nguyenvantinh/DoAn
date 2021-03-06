using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
      
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KhachHang kh)
        {
            if ((!string.IsNullOrEmpty(kh.TaiKhoan)) && (!string.IsNullOrEmpty(kh.MatKhau)))
            {
                KhachHang khachHang = dbContext.KhachHangs.SingleOrDefault(n => n.TaiKhoan == kh.TaiKhoan && n.MatKhau == kh.MatKhau);
                if (khachHang != null)
                {
                    Session["KhachHang"] = khachHang;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session["KhachHang"] = null;
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult SignUp(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                dbContext.KhachHangs.Add(kh);
                dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}