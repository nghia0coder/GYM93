using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM93.Migrations
{
    /// <inheritdoc />
    public partial class alterHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThangDangKy",
                table: "HoaDon",
                newName: "SoNgayDangKy");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoNgayDangKy",
                table: "HoaDon",
                newName: "ThangDangKy");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
