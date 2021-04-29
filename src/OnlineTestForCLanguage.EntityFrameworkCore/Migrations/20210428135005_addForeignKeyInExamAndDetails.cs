using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTestForCLanguage.Migrations
{
    public partial class addForeignKeyInExamAndDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExamDetails_ExamId",
                table: "ExamDetails",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDetails_Exams_ExamId",
                table: "ExamDetails",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamDetails_Exams_ExamId",
                table: "ExamDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExamDetails_ExamId",
                table: "ExamDetails");
        }
    }
}
