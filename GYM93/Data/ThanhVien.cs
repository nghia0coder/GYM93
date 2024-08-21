using System;
using System.Collections.Generic;

namespace GYM93.Data;

public partial class ThanhVien
{
    public int ThanhVienId { get; set; }

    public string? HoVaTenDem { get; set; }

    public string Ten { get; set; } = null!;

    public string Sđt { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public string Cmnd { get; set; } = null!;

    public DateTime NgayThamGia { get; set; }

    public string? HinhAnhTv { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
