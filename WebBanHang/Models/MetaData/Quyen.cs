using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [MetadataType(typeof(QuyenMetaData))]
    public partial class Quyen
    {
    }
    public class QuyenMetaData
    {
        [DisplayName("Mã quyền")]
        [Required(ErrorMessage ="Nhập mã quyền")]
        public string MaQuyen { get; set; }

        [DisplayName("Tên quyền")]
        [Required(ErrorMessage = "Nhập tên quyền")]
        public string TenQuyen { get; set; }
    }
}