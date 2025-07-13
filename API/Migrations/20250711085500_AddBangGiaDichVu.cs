using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddBangGiaDichVu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BangGiaDichVus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DichVuID = table.Column<int>(type: "int", nullable: false),
                    ThoiLuong = table.Column<int>(type: "int", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    KieuTinhGia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangGiaDichVus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BangGiaDichVus_DichVus_DichVuID",
                        column: x => x.DichVuID,
                        principalTable: "DichVus",
                        principalColumn: "DichVuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BangGiaDichVus_DichVuID",
                table: "BangGiaDichVus",
                column: "DichVuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BangGiaDichVus");
        }
    }
}
