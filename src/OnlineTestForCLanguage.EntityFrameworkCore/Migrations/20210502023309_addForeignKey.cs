using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTestForCLanguage.Migrations
{
    public partial class addForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tests_PaperId",
                table: "Tests",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_TestId",
                table: "TestDetails",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetail_Exams_ExamId",
                table: "TestDetail_Exams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetail_Exams_TestDetailId",
                table: "TestDetail_Exams",
                column: "TestDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PaperDetails_ExamId",
                table: "PaperDetails",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperDetails_Exams_ExamId",
                table: "PaperDetails",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestDetail_Exams_Exams_ExamId",
                table: "TestDetail_Exams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestDetail_Exams_TestDetails_TestDetailId",
                table: "TestDetail_Exams",
                column: "TestDetailId",
                principalTable: "TestDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestDetails_Tests_TestId",
                table: "TestDetails",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Papers_PaperId",
                table: "Tests",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperDetails_Exams_ExamId",
                table: "PaperDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDetail_Exams_Exams_ExamId",
                table: "TestDetail_Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDetail_Exams_TestDetails_TestDetailId",
                table: "TestDetail_Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDetails_Tests_TestId",
                table: "TestDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Papers_PaperId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_PaperId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_TestDetails_TestId",
                table: "TestDetails");

            migrationBuilder.DropIndex(
                name: "IX_TestDetail_Exams_ExamId",
                table: "TestDetail_Exams");

            migrationBuilder.DropIndex(
                name: "IX_TestDetail_Exams_TestDetailId",
                table: "TestDetail_Exams");

            migrationBuilder.DropIndex(
                name: "IX_PaperDetails_ExamId",
                table: "PaperDetails");
        }
    }
}
