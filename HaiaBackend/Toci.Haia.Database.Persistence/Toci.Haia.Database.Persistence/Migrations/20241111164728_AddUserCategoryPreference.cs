using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCategoryPreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCategoryPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    ReactionCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategoryPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCategoryPreferences_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCategoryPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9189));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9191));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9194));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9195));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9196));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9205));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9246));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9249));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9251));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9253));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9255));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9256));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9258));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9260));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(1136));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 47, 28, 578, DateTimeKind.Utc).AddTicks(9366));

            migrationBuilder.CreateIndex(
                name: "IX_UserCategoryPreferences_CategoryId",
                table: "UserCategoryPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCategoryPreferences_UserId",
                table: "UserCategoryPreferences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCategoryPreferences");

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5079));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5084));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5110));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5112));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5113));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5117));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5583));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5587));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5589));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5590));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5592));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5594));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5595));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5597));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5598));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5601));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 152, DateTimeKind.Utc).AddTicks(4582));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 152, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5800));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5801));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5755));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5758));
        }
    }
}
