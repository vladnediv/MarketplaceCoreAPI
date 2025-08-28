using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class modifyProductQuestionandProductReview : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advantages",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Disadvantages",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ProductQuestions");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "ProductQuestions");

            migrationBuilder.AddColumn<int>(
                name: "ProductReviewId",
                table: "MediaFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ProductReviewId",
                table: "MediaFiles",
                column: "ProductReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_ProductReviews_ProductReviewId",
                table: "MediaFiles",
                column: "ProductReviewId",
                principalTable: "ProductReviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_ProductReviews_ProductReviewId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_ProductReviewId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "ProductReviewId",
                table: "MediaFiles");

            migrationBuilder.AddColumn<string>(
                name: "Advantages",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Disadvantages",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ProductQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "ProductQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
