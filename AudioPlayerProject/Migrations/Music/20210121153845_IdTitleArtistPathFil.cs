using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayerProject.Migrations.Music
{
    public partial class IdTitleArtistPathFil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Music",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "MusicTitle",
                table: "Music");

            migrationBuilder.RenameColumn(
                name: "MusicPath",
                table: "Music",
                newName: "Path");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Music",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "Music",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Music",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Music",
                table: "Music",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Music",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "Artist",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Music");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Music",
                newName: "MusicPath");

            migrationBuilder.AddColumn<string>(
                name: "MusicTitle",
                table: "Music",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Music",
                table: "Music",
                column: "MusicTitle");
        }
    }
}
