using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updateehoadơn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hoaDons_AspNetUsers_UserID",
                table: "hoaDons");

            migrationBuilder.DropIndex(
                name: "IX_hoaDons_UserID",
                table: "hoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "hoaDons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "hoaDons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDons_UserID",
                table: "hoaDons",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_hoaDons_AspNetUsers_UserID",
                table: "hoaDons",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
