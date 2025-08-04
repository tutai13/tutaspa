using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Inventory_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InventoryHistories",
                newName: "HistoryId");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentSellingPrice",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "InventoryHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ActionType",
                table: "InventoryHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "InventoryHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductBatches",
                columns: table => new
                {
                    BatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemainingQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBatches", x => x.BatchId);
                    table.ForeignKey(
                        name: "FK_ProductBatches_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryHistories_BatchId",
                table: "InventoryHistories",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_ProductId",
                table: "ProductBatches",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryHistories_ProductBatches_BatchId",
                table: "InventoryHistories",
                column: "BatchId",
                principalTable: "ProductBatches",
                principalColumn: "BatchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryHistories_ProductBatches_BatchId",
                table: "InventoryHistories");

            migrationBuilder.DropTable(
                name: "ProductBatches");

            migrationBuilder.DropIndex(
                name: "IX_InventoryHistories_BatchId",
                table: "InventoryHistories");

            migrationBuilder.DropColumn(
                name: "CurrentSellingPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "InventoryHistories");

            migrationBuilder.RenameColumn(
                name: "HistoryId",
                table: "InventoryHistories",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "InventoryHistories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActionType",
                table: "InventoryHistories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
