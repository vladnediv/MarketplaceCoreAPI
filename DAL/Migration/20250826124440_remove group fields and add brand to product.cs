using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class removegroupfieldsandaddbrandtoproduct : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ProductCharacteristics");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ProductCharacteristics",
                type: "int",
                nullable: true);
        }
    }
}
