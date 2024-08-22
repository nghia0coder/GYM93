using System;
using System.Collections.Generic;
using GYM93.Data;

namespace GYM93.Models;

public partial class ThanhVien
{
    public int ThanhVienId { get; set; }

    public string? HoVaTenDem { get; set; }

    public string Ten { get; set; } = null!;

    public string Sđt { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public string BienSoXe { get; set; } = null!;

    public DateTime NgayThamGia { get; set; }

    public string? HinhAnhTv { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
