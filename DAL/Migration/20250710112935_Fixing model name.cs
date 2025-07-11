using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class Fixingmodelname : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductAttributeId",
                table: "KeyValue");

            migrationBuilder.RenameColumn(
                name: "ProductAttributeId",
                table: "KeyValue",
                newName: "ProductDescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_KeyValue_ProductAttributeId",
                table: "KeyValue",
                newName: "IX_KeyValue_ProductDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductDescriptionId",
                table: "KeyValue",
                column: "ProductDescriptionId",
                principalTable: "ProductDescriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductDescriptionId",
                table: "KeyValue");

            migrationBuilder.RenameColumn(
                name: "ProductDescriptionId",
                table: "KeyValue",
                newName: "ProductAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_KeyValue_ProductDescriptionId",
                table: "KeyValue",
                newName: "IX_KeyValue_ProductAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyValue_ProductDescriptions_ProductAttributeId",
                table: "KeyValue",
                column: "ProductAttributeId",
                principalTable: "ProductDescriptions",
                principalColumn: "Id");
        }
    }
}
