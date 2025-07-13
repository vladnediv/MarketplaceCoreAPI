using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class optimize_product : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductDescriptionId",
                table: "KeyValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescriptions_Products_ProductId",
                table: "ProductDescriptions");

            migrationBuilder.DropTable(
                name: "CurrentVariations");

            migrationBuilder.DropTable(
                name: "ProductVariations");

            migrationBuilder.DropIndex(
                name: "IX_KeyValue_ProductDescriptionId",
                table: "KeyValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDescriptions",
                table: "ProductDescriptions");

            migrationBuilder.DropColumn(
                name: "ProductDescriptionId",
                table: "KeyValue");

            migrationBuilder.RenameTable(
                name: "ProductDescriptions",
                newName: "ProductCharacteristics");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDescriptions_ProductId",
                table: "ProductCharacteristics",
                newName: "IX_ProductCharacteristics_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductCharacteristicId",
                table: "KeyValue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCharacteristics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ProductCharacteristics",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_KeyValue_ProductCharacteristicId",
                table: "KeyValue",
                column: "ProductCharacteristicId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyValue_ProductCharacteristics_ProductCharacteristicId",
                table: "KeyValue",
                column: "ProductCharacteristicId",
                principalTable: "ProductCharacteristics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyValue_ProductCharacteristics_ProductCharacteristicId",
                table: "KeyValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCharacteristics_Products_ProductId",
                table: "ProductCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_KeyValue_ProductCharacteristicId",
                table: "KeyValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCharacteristics",
                table: "ProductCharacteristics");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductCharacteristicId",
                table: "KeyValue");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ProductCharacteristics");

            migrationBuilder.RenameTable(
                name: "ProductCharacteristics",
                newName: "ProductDescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCharacteristics_ProductId",
                table: "ProductDescriptions",
                newName: "IX_ProductDescriptions_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorName",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProductDescriptionId",
                table: "KeyValue",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductDescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDescriptions",
                table: "ProductDescriptions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CurrentVariations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    VariationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentVariations_KeyValue_VariationId",
                        column: x => x.VariationId,
                        principalTable: "KeyValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentVariations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyValue_ProductDescriptionId",
                table: "KeyValue",
                column: "ProductDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentVariations_ProductId",
                table: "CurrentVariations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentVariations_VariationId",
                table: "CurrentVariations",
                column: "VariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_ProductId",
                table: "ProductVariations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductDescriptionId",
                table: "KeyValue",
                column: "ProductDescriptionId",
                principalTable: "ProductDescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescriptions_Products_ProductId",
                table: "ProductDescriptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
