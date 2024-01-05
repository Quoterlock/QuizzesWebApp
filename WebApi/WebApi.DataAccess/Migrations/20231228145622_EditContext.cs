using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Quiz_QuizId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.RenameTable(
                name: "Quiz",
                newName: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameIndex(
                name: "IX_Question_QuizId",
                table: "Questions",
                newName: "IX_Questions_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Option_QuestionId",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "Quiz");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuizId",
                table: "Question",
                newName: "IX_Question_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "Option",
                newName: "IX_Option_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Quiz_QuizId",
                table: "Question",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
