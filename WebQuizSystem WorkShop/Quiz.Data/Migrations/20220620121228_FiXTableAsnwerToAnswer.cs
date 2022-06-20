using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiz.Data.Migrations
{
    public partial class FiXTableAsnwerToAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asnwers_Questions_QuestionId",
                table: "Asnwers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Asnwers_AnswerId",
                table: "UserAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asnwers",
                table: "Asnwers");

            migrationBuilder.RenameTable(
                name: "Asnwers",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_Asnwers_QuestionId",
                table: "Answers",
                newName: "IX_Answers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Answers_AnswerId",
                table: "UserAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Answers_AnswerId",
                table: "UserAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Asnwers");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionId",
                table: "Asnwers",
                newName: "IX_Asnwers_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asnwers",
                table: "Asnwers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Asnwers_Questions_QuestionId",
                table: "Asnwers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Asnwers_AnswerId",
                table: "UserAnswers",
                column: "AnswerId",
                principalTable: "Asnwers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
