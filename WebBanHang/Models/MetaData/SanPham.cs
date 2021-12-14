using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(SanPhamMetaData))]
    public partial class SanPham
    {
    }
    public class SanPhamMetaData
    {
        [DisplayName("Mã sản phẩm")]
        public int MaSP { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Nhập tên sản phẩm.")]
        public string TenSP { get; set; }

        [DisplayName("Đơn giá")]
        [Required(ErrorMessage = "Nhập đơn giá.")]
        //[Range(0, int.MaxValue, ErrorMessage = "")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? DonGia { get; set; }

        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
        //[DataType(DataType.Date)]
        public DateTime? NgayCapNhap { get; set; }

        [DisplayName("Cấu hình")]
        [Required(ErrorMessage = "Nhập cấu hình sản phẩm.")]
        public string CauHinh { get; set; }

        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Nhập mô tả sản phẩm.")]
        public string MoTa { get; set; }

        [DisplayName("Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ nhập số")]
        public int? SoLuongTon { get; set; }

        [DisplayName("Lượt xem")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ nhập số")]
        public int? LuotXem { get; set; }

        [DisplayName("Lượt bình chọn")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ nhập số")]
        public int? LuotBinhChon { get; set; }

        [DisplayName("Lượt bình luận")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ nhập số")]
        public int? LuotBinhLuan { get; set; }

        [DisplayName("Số lần mua")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ nhập số")]
        public int? SoLanMua { get; set; }

        [DisplayName("Mới")]
        public int? Moi { get; set; } // ko dùng

        [DisplayName("Nhà cung cấp")]
        [Required(ErrorMessage = "Chọn nhà cung cấp.")]
        public int? MaNCC { get; set; }

        [DisplayName("Nhà sản xuất")]
        [Required(ErrorMessage = "Chọn nhà sản xuất.")]
        public int? MaNSX { get; set; }

        [DisplayName("Loại sản phẩm")]
        [Required(ErrorMessage = "Chọn loại sản phẩm.")]
        public int? MaLoaiSP { get; set; }

        [DisplayName("Đã xóa")]
        public bool? DaXoa { get; set; } 

        [DisplayName("Hình ảnh")]
        //[Required(ErrorMessage = "Chọn hình ảnh sản phẩm.")]
        public string HinhAnh { get; set; }

        [DisplayName("Hình ảnh 1")]
        //[Required(ErrorMessage = "Chọn hình ảnh sản phẩm.")]
        public string HinhAnh1 { get; set; }

        [DisplayName("Hình ảnh 2")]
        //[Required(ErrorMessage = "Chọn hình ảnh sản phẩm.")]
        public string HinhAnh2 { get; set; }

        //[Required(ErrorMessage = "Chọn hình ảnh sản phẩm.")]
        [DisplayName("Hình ảnh 3")]
        public string HinhAnh3 { get; set; }

        [DisplayName("Hình ảnh 4")]
        //[Required(ErrorMessage = "Chọn hình ảnh sản phẩm.")]
        public string HinhAnh4 { get; set; }
    }
}