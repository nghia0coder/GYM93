using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM93.Models;

public partial class ThanhVien
{
    public int ThanhVienId { get; set; }
    [Display(Name ="Họ Và Tên Đệm")]
    [MinLength(3, ErrorMessage = "Họ và Tên Đệm phải có ít nhất 3 ký tự.")]
    public string? HoVaTenDem { get; set; }
    [Display(Name = "Tên Thành Viên")]
    [Required(ErrorMessage = "Vui Lòng Nhập Tên")] 
    public string Ten { get; set; } = null!;
    [Display(Name = "Số Điện Thoại")]
 
    public string Sđt { get; set; } = null!;
    [Display(Name = "Giới Tính")]

    public bool GioiTinh { get; set; }
    [Display(Name = "Biển Số Xe")]
    public string BienSoXe { get; set; } = null!;
    [Display(Name = "Ngày Tham Gia Câu Lạc Bộ")]
  
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? NgayThamGia { get; set; }

    [Display(Name = "Ngày Hội Viên Bắt Đầu")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? NgayBatDau { get; set; }

    [Display(Name = "Thời Gian Kết Thúc Hội Viên")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? NgayKetThuc { get; set; }
    public decimal SoTienDaDong { get; set; }
    [Display(Name = "Hình Ảnh Thành Viên")]
    public string? HinhAnhTv { get; set; }
    [Display(Name = "Hình Ảnh Thành Viên")]
    [NotMapped]
    public IFormFile Image {  get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
