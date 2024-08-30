using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoToPostGenerationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAudioHasVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasVideo",
                table: "Audios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVideo",
                table: "Audios");
        }
    }
}
