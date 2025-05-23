﻿// <auto-generated />
using System;
using GYM93.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GYM93.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240827071124_UpdateThanhVienAndHoaDon")]
    partial class UpdateThanhVienAndHoaDon
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GYM93.Models.HoaDon", b =>
                {
                    b.Property<int>("HoaDonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HoaDonId"));

                    b.Property<DateTime>("NgayBatDau")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("NgayKetThuc")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("NgayThanhToan")
                        .HasColumnType("datetime2");

                    b.Property<int>("ThangDangKy")
                        .HasColumnType("int");

                    b.Property<int?>("ThanhVienId")
                        .HasColumnType("int");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("HoaDonId");

                    b.HasIndex("ThanhVienId");

                    b.ToTable("HoaDon", (string)null);
                });

            modelBuilder.Entity("GYM93.Models.ThanhVien", b =>
                {
                    b.Property<int>("ThanhVienId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThanhVienId"));

                    b.Property<string>("BienSoXe")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BienSoXe");

                    b.Property<bool>("GioiTinh")
                        .HasColumnType("bit");

                    b.Property<string>("HinhAnhTv")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("HinhAnhTV");

                    b.Property<string>("HoVaTenDem")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("NgayBatDau")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayKetThuc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NgayThamGia")
                        .HasColumnType("date");

                    b.Property<decimal>("SoTienDaDong")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Sđt")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("SĐT");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ThanhVienId");

                    b.ToTable("ThanhVien", (string)null);
                });

            modelBuilder.Entity("GYM93.Models.HoaDon", b =>
                {
                    b.HasOne("GYM93.Models.ThanhVien", "ThanhVien")
                        .WithMany("HoaDons")
                        .HasForeignKey("ThanhVienId")
                        .HasConstraintName("FK_HoaDon_ThanhVien");

                    b.Navigation("ThanhVien");
                });

            modelBuilder.Entity("GYM93.Models.ThanhVien", b =>
                {
                    b.Navigation("HoaDons");
                });
#pragma warning restore 612, 618
        }
    }
}
