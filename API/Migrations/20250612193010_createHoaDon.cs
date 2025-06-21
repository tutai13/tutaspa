using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class createHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hoaDons",
                columns: table => new
                {
                    HoaDonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaGiamGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongTienSauGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HinhThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    TienKhachDua = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TienThoiLai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NhanVienID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoaDons", x => x.HoaDonID);
                    table.ForeignKey(
                        name: "FK_hoaDons_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chiTietHoaDons",
                columns: table => new
                {
                    ChiTietHoaDonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoaDonID = table.Column<int>(type: "int", nullable: false),
                    SanPhamID = table.Column<int>(type: "int", nullable: true),
                    DichVuID = table.Column<int>(type: "int", nullable: true),
                    SoLuongSP = table.Column<int>(type: "int", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chiTietHoaDons", x => x.ChiTietHoaDonID);
                    table.ForeignKey(
                        name: "FK_chiTietHoaDons_DichVus_DichVuID",
                        column: x => x.DichVuID,
                        principalTable: "DichVus",
                        principalColumn: "DichVuID");
                    table.ForeignKey(
                        name: "FK_chiTietHoaDons_Product_SanPhamID",
                        column: x => x.SanPhamID,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_chiTietHoaDons_hoaDons_HoaDonID",
                        column: x => x.HoaDonID,
                        principalTable: "hoaDons",
                        principalColumn: "HoaDonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chiTietHoaDons_DichVuID",
                table: "chiTietHoaDons",
                column: "DichVuID");

            migrationBuilder.CreateIndex(
                name: "IX_chiTietHoaDons_HoaDonID",
                table: "chiTietHoaDons",
                column: "HoaDonID");

            migrationBuilder.CreateIndex(
                name: "IX_chiTietHoaDons_SanPhamID",
                table: "chiTietHoaDons",
                column: "SanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDons_UserID",
                table: "hoaDons",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chiTietHoaDons");

            migrationBuilder.DropTable(
                name: "hoaDons");
        }
    }
}
