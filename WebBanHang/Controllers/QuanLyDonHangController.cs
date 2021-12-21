﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        SellPhoneContext db = new SellPhoneContext();
        // GET: QuanLyDonHang
        public ActionResult ChoDuyet()
        {
            var lst = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult DaGiaoChuaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == false).OrderByDescending(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == true && n.TinhTrangGiaoHang == true).OrderByDescending(n => n.NgayDat);
            return View(lst);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds các sp của đơn hàng đó
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // Lấy dữ liệu của đơn hàng đó
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();

            // Lấy ds các sp của đơn hàng đó
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            if (ddhUpdate.TinhTrangGiaoHang == true && ddhUpdate.DaThanhToan == false)
            {
                GuiEmail("Xác nhận đơn hàng", "nvt0166@gmail.com", "smartstoreoffical@gmail.com", "tinh@1234", "Đơn hàng đang được giao đến bạn!");
            }
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chỉ gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true;   
            smtp.Send(mail);   //Gửi mail đi
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