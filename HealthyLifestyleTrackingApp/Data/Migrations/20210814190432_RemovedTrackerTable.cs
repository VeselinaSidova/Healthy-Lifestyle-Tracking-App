using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyLifestyleTrackingApp.Data.Migrations
{
    public partial class RemovedTrackerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedExercises_Trackers_TrackerId",
                table: "TrackedExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedFoods_Trackers_TrackerId",
                table: "TrackedFoods");

            migrationBuilder.DropTable(
                name: "Trackers");

            migrationBuilder.DropIndex(
                name: "IX_TrackedFoods_TrackerId",
                table: "TrackedFoods");

            migrationBuilder.DropIndex(
                name: "IX_TrackedExercises_TrackerId",
                table: "TrackedExercises");

            migrationBuilder.DropColumn(
                name: "TrackerId",
                table: "TrackedFoods");

            migrationBuilder.DropColumn(
                name: "TrackerId",
                table: "TrackedExercises");

            migrationBuilder.DropColumn(
                name: "TrackerId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackerId",
                table: "TrackedFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackerId",
                table: "TrackedExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFoods_TrackerId",
                table: "TrackedFoods",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedExercises_TrackerId",
                table: "TrackedExercises",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trackers_UserId",
                table: "Trackers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedExercises_Trackers_TrackerId",
                table: "TrackedExercises",
                column: "TrackerId",
                principalTable: "Trackers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedFoods_Trackers_TrackerId",
                table: "TrackedFoods",
                column: "TrackerId",
                principalTable: "Trackers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
