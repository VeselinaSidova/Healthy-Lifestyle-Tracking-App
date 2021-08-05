using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyLifestyleTrackingApp.Data.Migrations
{
    public partial class ImageUrlAddedToFoodAndExerciseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Exercises");
        }
    }
}
