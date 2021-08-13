using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyLifestyleTrackingApp.Data.Migrations
{
    public partial class ChangedLifeCoachesTableRemovedSuperUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuperUsers");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "LifeCoaches",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "LifeCoaches");

            migrationBuilder.CreateTable(
                name: "SuperUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LifeCoachId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuperUsers_LifeCoaches_LifeCoachId",
                        column: x => x.LifeCoachId,
                        principalTable: "LifeCoaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuperUsers_LifeCoachId",
                table: "SuperUsers",
                column: "LifeCoachId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperUsers_UserId",
                table: "SuperUsers",
                column: "UserId",
                unique: true);
        }
    }
}
