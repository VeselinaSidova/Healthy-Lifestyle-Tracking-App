using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyLifestyleTrackingApp.Data.Migrations
{
    public partial class AddedTrackerTrackedFoodTrackedExerciseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackerId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Trackers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trackers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackedExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackerId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DateTracked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedExercises_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedExercises_Trackers_TrackerId",
                        column: x => x.TrackerId,
                        principalTable: "Trackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackedFoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackerId = table.Column<int>(type: "int", nullable: false),
                    AmountInGrams = table.Column<double>(type: "float", nullable: false),
                    MealType = table.Column<int>(type: "int", nullable: false),
                    DateTracked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedFoods_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedFoods_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedFoods_Trackers_TrackerId",
                        column: x => x.TrackerId,
                        principalTable: "Trackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedExercises_ExerciseId",
                table: "TrackedExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedExercises_TrackerId",
                table: "TrackedExercises",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedExercises_UserId",
                table: "TrackedExercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFoods_FoodId",
                table: "TrackedFoods",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFoods_TrackerId",
                table: "TrackedFoods",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFoods_UserId",
                table: "TrackedFoods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trackers_UserId",
                table: "Trackers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedExercises");

            migrationBuilder.DropTable(
                name: "TrackedFoods");

            migrationBuilder.DropTable(
                name: "Trackers");

            migrationBuilder.DropColumn(
                name: "TrackerId",
                table: "AspNetUsers");
        }
    }
}
