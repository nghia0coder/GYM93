using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM93.Models;

public partial class HoaDon
{
    [DisplayName("Mã Thanh Toán")]   
    
    public int HoaDonId { get; set; }
    [DisplayName("Thành Viên Thanh Toán")]
    [Required(ErrorMessage ="Thành Viên Không Tồn Tại")]
    public int? ThanhVienId { get; set; }
    public string TenNguoiThanhToan { get; set; }
    [DisplayName("Tổng Tiền")]
    [Required(ErrorMessage = "Vui Lòng Nhập Số Tiền Thanh Toán")]
    public decimal TongTien { get; set; }
    [DisplayName("Ngày Thanh Toán")]
    public DateTime NgayThanhToan { get; set; }

    [NotMapped]
    [DisplayName("Số Tháng Đăng Ký")]
    [Required(ErrorMessage ="Vui Lòng Nhập Số Tháng Đăng Ký")]
    [Range(1, 12, ErrorMessage = "Số Tháng Đăng Ký Phải Nằm Trong Khoảng Từ {1} đến {2}")]
    public int ThangDangKy { get; set; }

    [DisplayName("Số Ngày Đăng Ký")]
	[Required(ErrorMessage = "Vui Lòng Nhập Số Ngày Đăng Ký")]
	public int SoNgayDangKy { get; set; }

    [DisplayName("Thành Viên Thanh Toán")]

    public virtual ThanhVien ThanhVien { get; set; }
  

}
