using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class removeProductDeliveryOptionandmodifyDeliveryOption : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDeliveryOptions");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DeliveryOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOptions_ProductId",
                table: "DeliveryOptions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOptions_Products_ProductId",
                table: "DeliveryOptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOptions_Products_ProductId",
                table: "DeliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOptions_ProductId",
                table: "DeliveryOptions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DeliveryOptions");

            migrationBuilder.CreateTable(
                name: "ProductDeliveryOptions",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DeliveryOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDeliveryOptions", x => new { x.ProductId, x.DeliveryOptionId });
                    table.ForeignKey(
                        name: "FK_ProductDeliveryOptions_DeliveryOptions_DeliveryOptionId",
                        column: x => x.DeliveryOptionId,
                        principalTable: "DeliveryOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDeliveryOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDeliveryOptions_DeliveryOptionId",
                table: "ProductDeliveryOptions",
                column: "DeliveryOptionId");
        }
    }
}
