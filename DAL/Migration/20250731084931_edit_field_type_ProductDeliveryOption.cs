using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class edit_field_type_ProductDeliveryOption : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryPrice",
                table: "ProductDeliveryOptions");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "DeliveryOptions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryPrice",
                table: "ProductDeliveryOptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "DeliveryOptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
