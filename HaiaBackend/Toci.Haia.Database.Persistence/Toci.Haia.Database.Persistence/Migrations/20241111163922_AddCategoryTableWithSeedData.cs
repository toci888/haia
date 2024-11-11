using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTableWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserGroupId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    TimeSpentMilliseconds = table.Column<int>(type: "integer", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostInteractions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostInteractions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Satyra" },
                    { 2, "Parodia" },
                    { 3, "Ironia" },
                    { 4, "Humor czarny" },
                    { 5, "Humor absurdalny" },
                    { 6, "Słowna gra" },
                    { 7, "Karykatura" },
                    { 8, "Humor polityczny" }
                });

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
                columns: new[] { "CreatedAt", "UserGroupId" },
                values: new object[] { new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5755), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserGroupId" },
                values: new object[] { new DateTime(2024, 11, 11, 16, 39, 22, 154, DateTimeKind.Utc).AddTicks(5758), null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupId",
                table: "Users",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PostInteractions_PostId",
                table: "PostInteractions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostInteractions_UserId",
                table: "PostInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GroupId",
                table: "Posts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_UserGroupId",
                table: "Users",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_UserGroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "PostInteractions");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1173));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1177));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1178));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1179));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1181));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1182));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1183));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1186));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1225));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1228));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1229));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1230));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1231));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1238));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 598, DateTimeKind.Utc).AddTicks(1274));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 598, DateTimeKind.Utc).AddTicks(1276));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1333));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1334));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1336));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 14, 30, 27, 599, DateTimeKind.Utc).AddTicks(1319));
        }
    }
}
