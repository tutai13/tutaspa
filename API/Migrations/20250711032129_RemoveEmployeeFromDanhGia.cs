using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmployeeFromDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGiass_DichVus_DichVuID",
                table: "DanhGiass");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGiass_Employee_NhanVienId",
                table: "DanhGiass");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_DanhGiass_DichVuID",
                table: "DanhGiass");

            migrationBuilder.DropIndex(
                name: "IX_DanhGiass_NhanVienId",
                table: "DanhGiass");

            migrationBuilder.DropColumn(
                name: "DichVuID",
                table: "DanhGiass");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "DanhGiass");

            migrationBuilder.DropColumn(
                name: "NhanVienId",
                table: "DanhGiass");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_MaDichVu",
                table: "DanhGiass",
                column: "MaDichVu");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGiass_DichVus_MaDichVu",
                table: "DanhGiass",
                column: "MaDichVu",
                principalTable: "DichVus",
                principalColumn: "DichVuID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGiass_DichVus_MaDichVu",
                table: "DanhGiass");

            migrationBuilder.DropIndex(
                name: "IX_DanhGiass_MaDichVu",
                table: "DanhGiass");

            migrationBuilder.AddColumn<int>(
                name: "DichVuID",
                table: "DanhGiass",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "DanhGiass",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NhanVienId",
                table: "DanhGiass",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_DichVuID",
                table: "DanhGiass",
                column: "DichVuID");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_NhanVienId",
                table: "DanhGiass",
                column: "NhanVienId");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGiass_DichVus_DichVuID",
                table: "DanhGiass",
                column: "DichVuID",
                principalTable: "DichVus",
                principalColumn: "DichVuID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGiass_Employee_NhanVienId",
                table: "DanhGiass",
                column: "NhanVienId",
                principalTable: "Employee",
                principalColumn: "Id");
        }
    }
}
