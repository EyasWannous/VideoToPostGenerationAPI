using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoToPostGenerationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrimaryKeyPhrase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryKeyPhrase",
                table: "PostsOptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryKeyPhrase",
                table: "PostsOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
