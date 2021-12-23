using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(NhaSanXuatMetaData))]
    public partial class NhaSanXuat
    {
    }
    public class NhaSanXuatMetaData
    {
        [DisplayName("Mã nhà sản xuất")]
        public int MaNSX { get; set; }

        [DisplayName("Tên nhà sản xuất")]
        [Required(ErrorMessage ="Nhập tên nhà sản xuất")]
        public string TenNSX { get; set; }
        public string ThongTin { get; set; }
        public string Logo { get; set; }
    }
}