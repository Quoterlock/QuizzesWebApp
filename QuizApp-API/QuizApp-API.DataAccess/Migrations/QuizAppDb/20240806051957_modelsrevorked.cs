using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp_API.DataAccess.Migrations.QuizAppDb
{
    /// <inheritdoc />
    public partial class modelsrevorked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Results",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Results",
                newName: "Username");
        }
    }
}
