using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_Project_Football.Migrations
{
    public partial class addlistPlayersproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_StatisticId",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_StatisticId",
                table: "Players",
                column: "StatisticId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_StatisticId",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_Players_StatisticId",
                table: "Players",
                column: "StatisticId",
                unique: true);
        }
    }
}
