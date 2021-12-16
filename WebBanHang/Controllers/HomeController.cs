using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        SellPhoneContext dbContext = new SellPhoneContext();
        public ActionResult Index()
        {
            ViewBag.lstSPNB = dbContext.SanPhams.Where(n => n.SPNoiBat == true);
            ViewBag.lstSPM = dbContext.SanPhams.Where(n => n.Moi == 1).Take(4).ToList();
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [ChildActionOnly]
        public ActionResult MenuPartial()
        {
            var lstSp = dbContext.SanPhams.Where(x => x.DaXoa == false);
            return PartialView(lstSp);
        }
    }
}