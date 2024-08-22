using System;
using System.Collections.Generic;
using GYM93.Models;
using Microsoft.EntityFrameworkCore;

namespace GYM93.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<ThanhVien> ThanhViens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NGHIANGHIA\\SQLSEVER2020EV;Initial Catalog=GYM93;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.ToTable("HoaDon");

            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");

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
            entity.Property(e => e.Ten).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
