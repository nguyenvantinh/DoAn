using Newtonsoft.Json;
using System.Data.Entity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHang.Models;
using PagedList;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace WebBanHang.Controllers
{
    public class AdminController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "AccessibleAdmin")]
        public ActionResult ThongKe(int thang = 12, int nam = 2021)
        {
            ViewBag.TongDoanhThu = ThongKeDoanhThu();
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongThanhVien = TongThanhVien();

            ViewBag.thang = thang;
            ViewBag.nam = nam;

            var lstDH = dbContext.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam).OrderBy(x => x.NgayDat.Value.Day).ToList();

            var lstNgay = lstDH.Select(r => r.NgayDat.Value.Day).Distinct().ToList();

            List<int> DoanhThuTheoNgay = new List<int>();
            var tong = 0;

            for (int i = 0; i < lstDH.Count; i++)
            {
                tong += int.Parse(lstDH[i].ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
                #region comments
                //if (i == lst.Count - 1 || lst[i].NgayDat.Value.Day != lst[i + 1].NgayDat.Value.Day)
                //{
                //    doanhthungay.Add(tong);
                //    tong = 0;
                //}
                //else
                //{

                //}
                #endregion

                if (i == lstDH.Count - 1)
                {
                    DoanhThuTheoNgay.Add(tong);
                    tong = 0;
                }
                else if (lstDH[i].NgayDat.Value.Day == lstDH[i + 1].NgayDat.Value.Day)
                {

                }
                else
                {
                    DoanhThuTheoNgay.Add(tong);
                    tong = 0;
                }

            }
            ViewBag.LstNgay = lstNgay;
            ViewBag.LstDoanhThuTheoNgay = DoanhThuTheoNgay;
            return View();
        }

        public decimal? ThongKeDoanhThu()
        {
            decimal? TongDoanhThu = dbContext.ChiTietDonDatHangs.Sum(n => n.DonGia * n.SoLuong);
            return TongDoanhThu;
        }

        public double ThongKeDonHang()
        {
            //Đếm đơn đặt hàng
            double sl = dbContext.DonDatHangs.Count();
            return sl;
        }

        public double TongThanhVien()
        {
            double slTV = dbContext.KhachHangs.Count();
            return slTV;
        }


        //[CustomAuthorize(Roles = "QuanTri")] // ko có quyền >> trả về trang dc style riêng
        [Authorize]// ko có quyền >> trả về trang mặc định 
        public ActionResult AddUser()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                CustomMembership Membership = new CustomMembership();
                //ValidateUser: check xem trong bảng NguoiDung có bản ghi nào có UserName và Password trùng với UserName và Password nhập vào không
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    // lấy ra đối tượng đó
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    NguoiDung nd = dbContext.NguoiDungs.SingleOrDefault(n => n.MaNguoiDung == user.UserId);
                    Session["NguoiDung"] = nd;
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            TaiKhoan = user.TaiKhoan,
                            HoTen = user.HoTen,
                            RoleName = user.Roles.Select(r => r.MaQuyen).ToList() // get ds các quyền của user, inited CustomMembershipUser in getuser 
                        };

                        string userData = JsonConvert.SerializeObject(userModel); // nén

                        // tạo một dối tượng FormsAuthenticationTicket: authTicket
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        // mã hóa đối tượng đó
                        string enTicket = FormsAuthentication.Encrypt(authTicket);

                        // tạo mới một cookie với value = đối tượng được mã hóa >> respone cái cookie đó
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                    }
                    return RedirectToAction("ThongKe", "Admin");
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or DiaChi invalid ^_^ ");
            return View(loginView);
        }

        #region Registration
        //[CustomAuthorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                //string userName = Membership.GetUserNameByEmail(registrationView.Email);
                var user = Membership.GetUser(registrationView.TaiKhoan, false);
                if (user == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                    return View(registrationView);
                }

                //Save User Data
                using (SellPhoneContext dbContext = new SellPhoneContext())
                {
                    var nguoidung = new NguoiDung()
                    {
                        TaiKhoan = registrationView.TaiKhoan,
                        MatKhau = registrationView.MatKhau,
                        HoTen = registrationView.HoTen,
                        Email = registrationView.Email,
                        DiaChi = registrationView.DiaChi,
                        SoDienThoai = registrationView.SoDienThoai
                    };

                    dbContext.NguoiDungs.Add(nguoidung);
                    dbContext.SaveChanges();
                }

                //Verification Email
                //VerificationEmail(registrationView.Email, registrationView.ActivationCode.ToString());
                messageRegistration = "Your account has been created successfully. ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);
        }
        #endregion
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin", null);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                    dbContext.Dispose();
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}