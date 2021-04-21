using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTestForCLanguage.Migrations
{
    public partial class changetesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Tests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPoints",
                table: "Tests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
