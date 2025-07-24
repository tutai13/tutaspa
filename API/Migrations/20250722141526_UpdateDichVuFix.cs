using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDichVuFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_BangGiaDichVus_BangGiaDichVuID",
                table: "DichVus");

            migrationBuilder.DropIndex(
                name: "IX_DichVus_BangGiaDichVuID",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "BangGiaDichVuID",
                table: "DichVus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BangGiaDichVuID",
                table: "DichVus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_BangGiaDichVuID",
                table: "DichVus",
                column: "BangGiaDichVuID");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_BangGiaDichVus_BangGiaDichVuID",
                table: "DichVus",
                column: "BangGiaDichVuID",
                principalTable: "BangGiaDichVus",
                principalColumn: "Id");
        }
    }
}
