using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndPreferencesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EducationLevel",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameSurname",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroupPostId",
                table: "Reactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupPostId",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Jokes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Friendships",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserId1 = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    PreferenceLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8173));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8179));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8181));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8183));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8185));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8187));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8189));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8191));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8193));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8285), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8280), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8288), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8287), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8289), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8288), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8290), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8289), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8292), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8291), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8293), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8292), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8295), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8294), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8296), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8295), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8298), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8297), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CommentTimestamp", "CreatedAt", "PostId" },
                values: new object[] { new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8299), new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8298), 0 });

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsAccepted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "CreatedAt" },
                values: new object[] { 0, new DateTime(2024, 11, 13, 21, 4, 11, 711, DateTimeKind.Utc).AddTicks(8688) });

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "CreatedAt" },
                values: new object[] { 0, new DateTime(2024, 11, 13, 21, 4, 11, 711, DateTimeKind.Utc).AddTicks(8693) });

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "GroupPostId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 2,
                column: "GroupPostId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 3,
                column: "GroupPostId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8478));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 13, 21, 4, 11, 714, DateTimeKind.Utc).AddTicks(8485));

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_GroupPostId",
                table: "Reactions",
                column: "GroupPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Jokes_CategoryId",
                table: "Jokes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryId",
                table: "Post",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_GroupId",
                table: "Post",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_CategoryId",
                table: "UserPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId1",
                table: "UserPreferences",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jokes_Categories_CategoryId",
                table: "Jokes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reactions_Posts_GroupPostId",
            //    table: "Reactions",
            //    column: "GroupPostId",
            //    principalTable: "Posts",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reactions_Users_UserId",
            //    table: "Reactions",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Post_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Jokes_Categories_CategoryId",
                table: "Jokes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_GroupPostId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Users_UserId",
                table: "Reactions");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_GroupPostId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Jokes_CategoryId",
                table: "Jokes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NameSurname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupPostId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "GroupPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Jokes");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(662));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(670));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(673));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(674));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(676));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(678));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(681));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(682));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(684));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(687));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(770));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(772));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(774));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(775));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(777));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(778));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(779));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(780));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(782));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(783));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 436, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 436, DateTimeKind.Utc).AddTicks(2099));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(1046));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(1053));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(1055));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "LastLogin", "PasswordHash", "UserGroupId", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(982), "user1@example.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hashed_password_1", null, "user1" },
                    { 2, new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(991), "user2@example.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hashed_password_2", null, "user2" }
                });
        }
    }
}
