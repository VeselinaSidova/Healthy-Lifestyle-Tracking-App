using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyLifestyleTrackingApp.Data.Migrations
{
    public partial class ExerciseTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "Exercises",
                newName: "CaloriesPerHour");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaloriesPerHour",
                table: "Exercises",
                newName: "Calories");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Exercises",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
