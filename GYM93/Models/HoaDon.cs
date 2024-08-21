using System;
using System.Collections.Generic;
using GYM93.Models;

namespace GYM93.Data;

public partial class HoaDon
{
    public int HoaDonId { get; set; }

    public int? ThanhVienId { get; set; }

    public decimal TongTien { get; set; }

    public DateTime NgayThanhToan { get; set; }

    public virtual ThanhVien? ThanhVien { get; set; }
}
