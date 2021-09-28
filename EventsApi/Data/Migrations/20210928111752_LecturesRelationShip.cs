using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsApi.Data.Migrations
{
    public partial class LecturesRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Speaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GitHub = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speaker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lecture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CodeEventId = table.Column<int>(type: "int", nullable: false),
                    SpeakerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecture_CodeEvent_CodeEventId",
                        column: x => x.CodeEventId,
                        principalTable: "CodeEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecture_Speaker_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speaker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_CodeEventId",
                table: "Lecture",
                column: "CodeEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_SpeakerId",
                table: "Lecture",
                column: "SpeakerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lecture");

            migrationBuilder.DropTable(
                name: "Speaker");
        }
    }
}
