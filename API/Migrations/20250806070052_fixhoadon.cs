using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class fixhoadon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TongTienSauGiamGia",
                table: "hoaDons",
                newName: "GiaTriGiam");

            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "hoaDons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hoaDons_VoucherID",
                table: "hoaDons",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_hoaDons_Voucher_VoucherID",
                table: "hoaDons",
                column: "VoucherID",
                principalTable: "Voucher",
                principalColumn: "VoucherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hoaDons_Voucher_VoucherID",
                table: "hoaDons");

            migrationBuilder.DropIndex(
                name: "IX_hoaDons_VoucherID",
                table: "hoaDons");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "hoaDons");

            migrationBuilder.RenameColumn(
                name: "GiaTriGiam",
                table: "hoaDons",
                newName: "TongTienSauGiamGia");
        }
    }
}
