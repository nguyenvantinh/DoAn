using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;
using PagedList;
using System.Web.Mvc;
using System.Net;

namespace WebBanHang.DAL
{
    public class SanPhamDA
    {
        SellPhoneContext db = null;
        public SanPhamDA()
        {
            db = new SellPhoneContext();
        }
        public IEnumerable<SanPham> GetSanPham(int Page, int PageSize, string Keyword)
        {
            IQueryable<SanPham> lstSanPham = db.SanPhams;
            if (!string.IsNullOrEmpty(Keyword))
            {
                lstSanPham = lstSanPham.Where(x => x.TenSP.Contains(Keyword) || x.MoTa.Contains(Keyword)||x.NhaSanXuat.TenNSX.Contains(Keyword)|| x.NhaCungCap.TenNCC.Contains(Keyword)&&x.DaXoa==false);
            }
            return lstSanPham.OrderBy(x => x.DaXoa).ToPagedList(Page, PageSize);
        }
        public SanPham GetSanPhamByID(int ID)
        {
            SanPham sp = new SanPham();
            sp = db.SanPhams.Where(x => x.DaXoa == false && x.MaSP == ID).FirstOrDefault();
            return sp;
        }
        public int Add(SanPham sp)
        {
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return sp.MaSP;
        }
    }
}