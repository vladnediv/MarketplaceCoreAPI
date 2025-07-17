using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migration
{
    /// <inheritdoc />
    public partial class test_migration : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductQuestionAnswer_ProductQuestions_QuestionId",
                table: "ProductQuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductQuestionAnswer",
                table: "ProductQuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "ProductQuestionAnswer",
                newName: "ProductQuestionAnswers");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_ProductQuestionAnswer_QuestionId",
                table: "ProductQuestionAnswers",
                newName: "IX_ProductQuestionAnswers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductQuestionAnswers",
                table: "ProductQuestionAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductQuestionAnswers_ProductQuestions_QuestionId",
                table: "ProductQuestionAnswers",
                column: "QuestionId",
                principalTable: "ProductQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Orders_OrderId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductQuestionAnswers_ProductQuestions_QuestionId",
                table: "ProductQuestionAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductQuestionAnswers",
                table: "ProductQuestionAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "ProductQuestionAnswers",
                newName: "ProductQuestionAnswer");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_ProductQuestionAnswers_QuestionId",
                table: "ProductQuestionAnswer",
                newName: "IX_ProductQuestionAnswer_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductQuestionAnswer",
                table: "ProductQuestionAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductQuestionAnswer_ProductQuestions_QuestionId",
                table: "ProductQuestionAnswer",
                column: "QuestionId",
                principalTable: "ProductQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
