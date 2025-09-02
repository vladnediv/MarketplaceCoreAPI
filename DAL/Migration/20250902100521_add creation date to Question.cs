using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class addcreationdatetoQuestion : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "ProductQuestions",
                type: "date",
                nullable: false,
                defaultValue: DateOnly.FromDateTime(DateTime.UtcNow));

            migrationBuilder.Sql(
                "UPDATE dbo.ProductQuestionAnswers SET CreatedAt = CAST(GETDATE() AS date);"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductQuestions");
        }
    }
}
