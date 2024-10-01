using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddParentChildComedyTextRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GptJoke",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GptJoke",
                table: "Comments");
        }
    }
}
