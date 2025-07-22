using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class Init : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDeliveryOptions",
                table: "ProductDeliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductDeliveryOptions_ProductId",
                table: "ProductDeliveryOptions");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDeliveryOptions",
                table: "ProductDeliveryOptions",
                columns: new[] { "ProductId", "DeliveryOptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDeliveryOptions_DeliveryOptionId",
                table: "ProductDeliveryOptions",
                column: "DeliveryOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDeliveryOptions",
                table: "ProductDeliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductDeliveryOptions_DeliveryOptionId",
                table: "ProductDeliveryOptions");

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercent",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDeliveryOptions",
                table: "ProductDeliveryOptions",
                columns: new[] { "DeliveryOptionId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDeliveryOptions_ProductId",
                table: "ProductDeliveryOptions",
                column: "ProductId");
        }
    }
}
