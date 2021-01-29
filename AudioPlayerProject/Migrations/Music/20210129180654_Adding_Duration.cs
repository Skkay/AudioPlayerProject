using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayerProject.Migrations.Music
{
    public partial class Adding_Duration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Music",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration", 
                table:"Music");
        }
    }
}
