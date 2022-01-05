using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(KhachHangMetaData))]
    public partial class KhachHang
    {
    }
    public class KhachHangMetaData
    {
        [DisplayName("Mã khách hàng")]
        [Required(ErrorMessage = "Bắt buộc nhập mã khách hàng")]
        public int MaKhachHang { get; set; }

        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Bắt buộc nhập họ tên")]
        public string HoTen { get; set; }

        [DisplayName("Giới tính")]
        [Required(ErrorMessage = "Bắt buộc nhập giới tính")]
        public string GioiTinh { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Bắt buộc nhập địa chỉ")]
        public string DiaChi { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Bắt buộc nhập email")]
        //[RegularExpression(@"^\S*$", ErrorMessage = "Email Address cannot have white spaces")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Nhập đúng định dạng email")]
        public string Email { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Bắt buộc nhập số điện thoại")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Bắt buộc nhập tài khoản")]
        public string TaiKhoan { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Bắt buộc nhập mật khẩu")]
        public string MatKhau { get; set; }
    }
}