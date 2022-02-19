using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(LoaiSanPhamMetaData))]
    public partial class LoaiSanPham
    {
    }
    public class LoaiSanPhamMetaData
    {
        [DisplayName("Mã loại sản phẩm")]
        public int MaLoaiSP { get; set; }

        [DisplayName("Tên loại sản phẩm")]
        [Required(ErrorMessage ="Nhập tên loại sản phẩm")]
        public string TenLoai { get; set; }
        
        [DisplayName("Bí danh")]
        [Required(ErrorMessage ="Nhập bí danh")]
        public string BiDanh { get; set; }

        [DisplayName("Giá từ")]
        [Required(ErrorMessage ="Nhập đơn giá")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Đơn giá không hợp lệ")]
        [Range(0, 9999999999999999.99)]
        public Nullable<decimal> FromPrice { get; set; }

        [DisplayName("Đến")]
        [Required(ErrorMessage = "Nhập đơn giá")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage ="Đơn giá không hợp lệ")]
        [Range(0, 9999999999999999.99)]
        public Nullable<decimal> ToPrice { get; set; }
    }
}