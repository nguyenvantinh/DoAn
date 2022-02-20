using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class tkController : Controller
    {
        // GET: tk
        SellPhoneContext dbContext = new SellPhoneContext();
        public ActionResult Index(int thang, int nam)
        {
            var lst = dbContext.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam).OrderBy(x=>x.NgayDat.Value.Day).ToList();

            var q = lst.Select(r => r.NgayDat.Value.Day).Distinct().ToList();

            List<int> doanhthungay = new List<int>();
            var tong = 0;

            for (int i = 0; i < lst.Count; i++)
            {
                tong += int.Parse(lst[i].ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());

                //if (i == lst.Count - 1 || lst[i].NgayDat.Value.Day != lst[i + 1].NgayDat.Value.Day)
                //{
                //    doanhthungay.Add(tong);
                //    tong = 0;
                //}
                //else
                //{

                //}
                
                if (i == lst.Count - 1)
                {
                    doanhthungay.Add(tong);
                    tong = 0;
                }
                else if (lst[i].NgayDat.Value.Day == lst[i + 1].NgayDat.Value.Day)
                {

                }
                else
                {
                    doanhthungay.Add(tong);
                    tong = 0;
                }

            }
            return View();
        }
    }
}