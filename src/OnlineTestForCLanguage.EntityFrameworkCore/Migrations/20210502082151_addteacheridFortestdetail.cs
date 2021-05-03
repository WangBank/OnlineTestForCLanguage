using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTestForCLanguage.Migrations
{
    public partial class addteacheridFortestdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "TestDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "TestDetails");
        }
    }
}
