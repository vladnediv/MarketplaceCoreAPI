using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class addpicturestoragetoquestionmodel : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrls",
                table: "ProductQuestions");

            migrationBuilder.AddColumn<int>(
                name: "ProductQuestionId",
                table: "MediaFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ProductQuestionId",
                table: "MediaFiles",
                column: "ProductQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_ProductQuestions_ProductQuestionId",
                table: "MediaFiles",
                column: "ProductQuestionId",
                principalTable: "ProductQuestions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_ProductQuestions_ProductQuestionId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_ProductQuestionId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "ProductQuestionId",
                table: "MediaFiles");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrls",
                table: "ProductQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
