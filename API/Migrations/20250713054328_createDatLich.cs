using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class createDatLich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datLiches",
                columns: table => new
                {
                    DatLichID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiLuong = table.Column<int>(type: "int", nullable: false),
                    DichVuID = table.Column<int>(type: "int", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaThanhToan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datLiches", x => x.DatLichID);
                    table.ForeignKey(
                        name: "FK_datLiches_DichVus_DichVuID",
                        column: x => x.DichVuID,
                        principalTable: "DichVus",
                        principalColumn: "DichVuID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_datLiches_DichVuID",
                table: "datLiches",
                column: "DichVuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datLiches");
        }
    }
}
