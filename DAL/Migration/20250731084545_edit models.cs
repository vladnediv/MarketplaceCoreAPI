using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class editmodels : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ProductQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "DeliveryOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ProductQuestions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DeliveryOptions");
        }
    }
}
