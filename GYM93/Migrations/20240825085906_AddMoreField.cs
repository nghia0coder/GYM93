using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM93.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayHetHan",
                table: "ThanhVien",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "SoTienDaDong",
                table: "ThanhVien",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDau",
                table: "HoaDon",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThuc",
                table: "HoaDon",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ThangDangKy",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayHetHan",
                table: "ThanhVien");

            migrationBuilder.DropColumn(
                name: "SoTienDaDong",
                table: "ThanhVien");

            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "NgayKetThuc",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "ThangDangKy",
                table: "HoaDon");

            migrationBuilder.RenameColumn(
                name: "BienSoXe",
                table: "ThanhVien",
                newName: "CMND");

            migrationBuilder.AlterColumn<string>(
                name: "HoVaTenDem",
                table: "ThanhVien",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
