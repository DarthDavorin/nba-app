using Microsoft.EntityFrameworkCore.Migrations;

namespace NbaApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NbaClubs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClubName = table.Column<string>(nullable: true),
                    ClubCity = table.Column<string>(nullable: true),
                    GP = table.Column<int>(nullable: false),
                    MIN = table.Column<float>(nullable: false),
                    PTS = table.Column<float>(nullable: false),
                    FGM = table.Column<float>(nullable: false),
                    FGA = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NbaClubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NbaPlayers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    GP = table.Column<int>(nullable: false),
                    MIN = table.Column<float>(nullable: false),
                    PTS = table.Column<float>(nullable: false),
                    FGM = table.Column<float>(nullable: false),
                    FGA = table.Column<float>(nullable: false),
                    ClubId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NbaPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NbaPlayers_NbaClubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "NbaClubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NbaPlayers_ClubId",
                table: "NbaPlayers",
                column: "ClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NbaPlayers");

            migrationBuilder.DropTable(
                name: "NbaClubs");
        }
    }
}
