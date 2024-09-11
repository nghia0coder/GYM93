using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM93.Migrations
{
    /// <inheritdoc />
    public partial class updateHoaDonAndAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaiKhoanId",
                table: "HoaDon",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_TaiKhoanId",
                table: "HoaDon",
                column: "TaiKhoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_ThanhVien",
                table: "HoaDon",
                column: "TaiKhoanId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_ThanhVien",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_TaiKhoanId",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "TaiKhoanId",
                table: "HoaDon");
        }
    }
}
