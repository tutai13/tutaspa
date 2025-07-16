using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updatedatlich : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_datLiches_DichVus_DichVuID",
                table: "datLiches");

            migrationBuilder.DropIndex(
                name: "IX_datLiches_DichVuID",
                table: "datLiches");

            migrationBuilder.DropColumn(
                name: "DichVuID",
                table: "datLiches");

            migrationBuilder.CreateTable(
                name: "chiTietDatLiches",
                columns: table => new
                {
                    ChiTietDatLichID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatLichID = table.Column<int>(type: "int", nullable: false),
                    DichVuID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chiTietDatLiches", x => x.ChiTietDatLichID);
                    table.ForeignKey(
                        name: "FK_chiTietDatLiches_DichVus_DichVuID",
                        column: x => x.DichVuID,
                        principalTable: "DichVus",
                        principalColumn: "DichVuID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chiTietDatLiches_datLiches_DatLichID",
                        column: x => x.DatLichID,
                        principalTable: "datLiches",
                        principalColumn: "DatLichID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chiTietDatLiches_DatLichID",
                table: "chiTietDatLiches",
                column: "DatLichID");

            migrationBuilder.CreateIndex(
                name: "IX_chiTietDatLiches_DichVuID",
                table: "chiTietDatLiches",
                column: "DichVuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chiTietDatLiches");

            migrationBuilder.AddColumn<int>(
                name: "DichVuID",
                table: "datLiches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_datLiches_DichVuID",
                table: "datLiches",
                column: "DichVuID");

            migrationBuilder.AddForeignKey(
                name: "FK_datLiches_DichVus_DichVuID",
                table: "datLiches",
                column: "DichVuID",
                principalTable: "DichVus",
                principalColumn: "DichVuID");
        }
    }
}
