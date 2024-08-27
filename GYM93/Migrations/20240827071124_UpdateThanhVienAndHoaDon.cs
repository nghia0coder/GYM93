using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM93.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThanhVienAndHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayHetHan",
                table: "ThanhVien");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayThamGia",
                table: "ThanhVien",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDau",
                table: "ThanhVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThuc",
                table: "ThanhVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayKetThuc",
                table: "HoaDon",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayBatDau",
                table: "HoaDon",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "ThanhVien");

            migrationBuilder.DropColumn(
                name: "NgayKetThuc",
                table: "ThanhVien");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayThamGia",
                table: "ThanhVien",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayHetHan",
                table: "ThanhVien",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayKetThuc",
                table: "HoaDon",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayBatDau",
                table: "HoaDon",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
