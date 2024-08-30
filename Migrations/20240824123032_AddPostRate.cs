using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoToPostGenerationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPostRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Posts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Posts");
        }
    }
}
