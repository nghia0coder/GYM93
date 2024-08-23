using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GYM93.Data;

namespace GYM93.Models;

public partial class ThanhVien
{
    public int ThanhVienId { get; set; }
    [Display(Name ="Họ Và Tên Đệm")]
    [Required(ErrorMessage ="Vui Lòng Nhập Họ Tên Đệm")]
    [MinLength(3, ErrorMessage = "Họ và Tên Đệm phải có ít nhất 3 ký tự.")]
    public string? HoVaTenDem { get; set; }
    [Display(Name = "Tên Thành Viên")]
    [Required(ErrorMessage = "Vui Lòng Nhập Tên")]
    public string Ten { get; set; } = null!;
    [Display(Name = "Số Điện Thoại")]
    [Required(ErrorMessage = "Vui Lòng Nhập Số Điện Thoại")]
    public string Sđt { get; set; } = null!;
    [Display(Name = "Giới Tính")]

    public bool GioiTinh { get; set; }
    [Display(Name = "Biển Số Xe")]
    [Required(ErrorMessage = "Vui Lòng Nhập Biển Số Xe")]
    public string BienSoXe { get; set; } = null!;
    [Display(Name = "Ngày Tham Gia")]

    public DateTime NgayThamGia { get; set; }
    [Display(Name = "Hình Ảnh Thành Viên")]

    public string? HinhAnhTv { get; set; }
    [Display(Name = "Hình Ảnh Thành Viên")]
    [NotMapped]
    [Required(ErrorMessage = "Yêu Cầu Hình Ảnh Thành Viên")]
    public IFormFile? Image {  get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
