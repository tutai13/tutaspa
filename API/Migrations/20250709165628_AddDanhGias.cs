using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    QuantityChanged = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryHistories_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDichVu = table.Column<int>(type: "int", nullable: false),
                    DichVuID = table.Column<int>(type: "int", nullable: false),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    NhanVienId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnDanh = table.Column<bool>(type: "bit", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGiass_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiass_DichVus_DichVuID",
                        column: x => x.DichVuID,
                        principalTable: "DichVus",
                        principalColumn: "DichVuID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiass_Employee_NhanVienId",
                        column: x => x.NhanVienId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_DichVuID",
                table: "DanhGiass",
                column: "DichVuID");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_NhanVienId",
                table: "DanhGiass",
                column: "NhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiass_UserId",
                table: "DanhGiass",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryHistories_ProductId",
                table: "InventoryHistories",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGiass");

            migrationBuilder.DropTable(
                name: "InventoryHistories");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
