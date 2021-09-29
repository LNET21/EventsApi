using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsApi.Data.Migrations
{
    public partial class NotRequiredSpeaker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Speaker_SpeakerId",
                table: "Lecture");

            migrationBuilder.AlterColumn<int>(
                name: "SpeakerId",
                table: "Lecture",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Speaker_SpeakerId",
                table: "Lecture",
                column: "SpeakerId",
                principalTable: "Speaker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Speaker_SpeakerId",
                table: "Lecture");

            migrationBuilder.AlterColumn<int>(
                name: "SpeakerId",
                table: "Lecture",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Speaker_SpeakerId",
                table: "Lecture",
                column: "SpeakerId",
                principalTable: "Speaker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
