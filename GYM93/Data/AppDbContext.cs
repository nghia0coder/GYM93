using System;
using System.Collections.Generic;
using GYM93.Models;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Data;

public partial class AppDbContext : DbContext
{   
    private readonly IConfiguration _configuration;
  

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<ThanhVien> ThanhViens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.ToTable("HoaDon");

            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NgayThanhToan).HasColumnType("datetime2");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

            entity.HasOne(d => d.ThanhVien).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.ThanhVienId)
                .HasConstraintName("FK_HoaDon_ThanhVien");
        });

        modelBuilder.Entity<ThanhVien>(entity =>
        {
            entity.ToTable("ThanhVien");

            entity.Property(e => e.BienSoXe)
                .HasMaxLength(50)
                .HasColumnName("BienSoXe");
            entity.Property(e => e.HinhAnhTv).HasColumnName("HinhAnhTV");
            entity.Property(e => e.HoVaTenDem).HasMaxLength(50);
            entity.Property(e => e.Sđt)
                .HasMaxLength(50)
                .HasColumnName("SĐT");
            entity.Property(e => e.NgayThamGia).HasColumnType("date");
            entity.Property(e => e.Ten).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
