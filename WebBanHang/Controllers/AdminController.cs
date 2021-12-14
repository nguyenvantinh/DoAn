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

namespace WebBanHang.Controllers
{
    public class AdminController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();

        [CustomAuthorize(Roles = "AccessibleAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        //[CustomAuthorize(Roles = "QuanTri")] // ko có quyền >> trả về trang dc style riêng
        [Authorize]// ko có quyền >> trả về trang mặc định 
        public ActionResult AddUser()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                CustomMembership Membership = new CustomMembership();
                //ValidateUser: check xem trong bảng NguoiDung có bản ghi nào có UserName và Password trùng với UserName và Password nhập vào không
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            TaiKhoan = user.TaiKhoan,
                            HoTen = user.HoTen,
                            RoleName = user.Roles.Select(r => r.MaQuyen).ToList() // get ds các quyền của user 
                        };

                        string userData = JsonConvert.SerializeObject(userModel);

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

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
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
    }
}