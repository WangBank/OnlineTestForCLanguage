using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTestForCLanguage.Migrations
{
    public partial class addPaperForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaperDetails_PaperId",
                table: "PaperDetails",
                column: "PaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperDetails_Papers_PaperId",
                table: "PaperDetails",
                column: "PaperId",
                principalTable: "Papers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperDetails_Papers_PaperId",
                table: "PaperDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaperDetails_PaperId",
                table: "PaperDetails");
        }
    }
}
