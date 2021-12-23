using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(NhaCungCapMetaData))]
    public partial class NhaCungCap
    {
    }
    public class NhaCungCapMetaData
    {
        [DisplayName("Mã NCC")]
        public int MaNCC { get; set; }

        [DisplayName("Tên nhà cung cấp")]
        [Required(ErrorMessage ="Nhập tên nhà cung cấp")]
        public string TenNCC { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string DiaChi { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Nhập email")]
        public string Email { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Nhập số điện thoại")]
        public string SoDienThoai { get; set; }
        public string Fax { get; set; }
    }
}