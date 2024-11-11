using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Interests = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComedyTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    CommentTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ParentTextId = table.Column<int>(type: "integer", nullable: true),
                    ChildTextId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComedyTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComedyTexts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FriendId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jokes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jokes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProviderUserId = table.Column<string>(type: "text", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JokeId = table.Column<int>(type: "integer", nullable: false),
                    CommentId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    CommentTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ComedyTextId = table.Column<int>(type: "integer", nullable: true),
                    ComedyTextId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ComedyTexts_ComedyTextId",
                        column: x => x.ComedyTextId,
                        principalTable: "ComedyTexts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_ComedyTexts_ComedyTextId1",
                        column: x => x.ComedyTextId1,
                        principalTable: "ComedyTexts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Jokes_JokeId",
                        column: x => x.JokeId,
                        principalTable: "Jokes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JokeId = table.Column<int>(type: "integer", nullable: true),
                    CommentId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Jokes_JokeId",
                        column: x => x.JokeId,
                        principalTable: "Jokes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ComedyTexts",
                columns: new[] { "Id", "Author", "ChildTextId", "CommentTimestamp", "ParentTextId", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, "Author_1", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2765), null, "Sample Comedy Text 1", null },
                    { 2, "Author_2", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2769), null, "Sample Comedy Text 2", null },
                    { 3, "Author_3", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2770), null, "Sample Comedy Text 3", null },
                    { 4, "Author_4", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2772), null, "Sample Comedy Text 4", null },
                    { 5, "Author_5", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2773), null, "Sample Comedy Text 5", null },
                    { 6, "Author_6", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2774), null, "Sample Comedy Text 6", null },
                    { 7, "Author_7", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2776), null, "Sample Comedy Text 7", null },
                    { 8, "Author_8", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2777), null, "Sample Comedy Text 8", null },
                    { 9, "Author_9", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2778), null, "Sample Comedy Text 9", null },
                    { 10, "Author_10", null, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2780), null, "Sample Comedy Text 10", null }
                });

            //migrationBuilder.InsertData(
            //    table: "Comments",
            //    columns: new[] { "Id", "Author", "ComedyTextId", "ComedyTextId1", "CommentId", "CommentTimestamp", "JokeId", "Text", "UserId" },
            //    values: new object[,]
            //    {
            //        { 1, "Commenter_1", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2810), 0, "Sample Comment 1", 0 },
            //        { 2, "Commenter_2", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2813), 0, "Sample Comment 2", 0 },
            //        { 3, "Commenter_3", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2815), 0, "Sample Comment 3", 0 },
            //        { 4, "Commenter_4", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2816), 0, "Sample Comment 4", 0 },
            //        { 5, "Commenter_5", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2818), 0, "Sample Comment 5", 0 },
            //        { 6, "Commenter_6", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2819), 0, "Sample Comment 6", 0 },
            //        { 7, "Commenter_7", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2820), 0, "Sample Comment 7", 0 },
            //        { 8, "Commenter_8", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2828), 0, "Sample Comment 8", 0 },
            //        { 9, "Commenter_9", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2829), 0, "Sample Comment 9", 0 },
            //        { 10, "Commenter_10", null, null, 0, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2830), 0, "Sample Comment 10", 0 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Friendships",
            //    columns: new[] { "Id", "FriendId", "UserId" },
            //    values: new object[,]
            //    {
            //        { 3, 4, 3 },
            //        { 4, 5, 4 },
            //        { 5, 6, 5 },
            //        { 6, 7, 6 },
            //        { 7, 8, 7 },
            //        { 8, 9, 8 },
            //        { 9, 10, 9 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Jokes",
            //    columns: new[] { "Id", "CreatedAt", "Text", "UserId" },
            //    values: new object[,]
            //    {
            //        { 1, new DateTime(2024, 11, 11, 13, 59, 13, 209, DateTimeKind.Utc).AddTicks(6480), "Dlaczego niebo jest niebieskie? Bo programista jeszcze nie skończył debugować!", 0 },
            //        { 2, new DateTime(2024, 11, 11, 13, 59, 13, 209, DateTimeKind.Utc).AddTicks(6483), "Dlaczego komputer był smutny? Bo miał zbyt dużo problemów!", 0 }
            //    });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Id", "Interests", "ProfilePictureUrl" },
                values: new object[,]
                {
                    { 1, "Interests_1", "https://example.com/user1.jpg" },
                    { 2, "Interests_2", "https://example.com/user2.jpg" },
                    { 3, "Interests_3", "https://example.com/user3.jpg" },
                    { 4, "Interests_4", "https://example.com/user4.jpg" },
                    { 5, "Interests_5", "https://example.com/user5.jpg" },
                    { 6, "Interests_6", "https://example.com/user6.jpg" },
                    { 7, "Interests_7", "https://example.com/user7.jpg" },
                    { 8, "Interests_8", "https://example.com/user8.jpg" },
                    { 9, "Interests_9", "https://example.com/user9.jpg" },
                    { 10, "Interests_10", "https://example.com/user10.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "LastLogin", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2909), "user1@example.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hashed_password_1", "user1" },
                    { 2, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2911), "user2@example.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hashed_password_2", "user2" }
                });

            //migrationBuilder.InsertData(
            //    table: "Friendships",
            //    columns: new[] { "Id", "FriendId", "UserId" },
            //    values: new object[,]
            //    {
            //        { 1, 2, 1 },
            //        { 2, 3, 2 },
            //        { 10, 1, 10 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Likes",
            //    columns: new[] { "Id", "CommentId", "UserId" },
            //    values: new object[,]
            //    {
            //        { 1, 1, 1 },
            //        { 2, 2, 2 },
            //        { 3, 3, 3 },
            //        { 4, 4, 4 },
            //        { 5, 5, 5 },
            //        { 6, 6, 6 },
            //        { 7, 7, 7 },
            //        { 8, 8, 8 },
            //        { 9, 9, 9 },
            //        { 10, 10, 10 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Reactions",
            //    columns: new[] { "Id", "CommentId", "JokeId", "ReactionType", "UserId" },
            //    values: new object[,]
            //    {
            //        { 1, null, 1, "like", 1 },
            //        { 2, null, 1, "superlike", 2 },
            //        { 3, null, 2, "meh", 1 }
            //    });

            migrationBuilder.InsertData(
                table: "SocialLogins",
                columns: new[] { "Id", "LinkedAt", "Provider", "ProviderUserId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2928), "Google", "google_user_1", 1 },
                    { 2, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2930), "Facebook", "facebook_user_1", 1 },
                    { 3, new DateTime(2024, 11, 11, 13, 59, 13, 210, DateTimeKind.Utc).AddTicks(2931), "GitHub", "github_user_2", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComedyTexts_UserId",
                table: "ComedyTexts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ComedyTextId",
                table: "Comments",
                column: "ComedyTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ComedyTextId1",
                table: "Comments",
                column: "ComedyTextId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_JokeId",
                table: "Comments",
                column: "JokeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Jokes_UserId",
                table: "Jokes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentId",
                table: "Likes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_JokeId",
                table: "Reactions",
                column: "JokeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_JokeId",
                table: "Reactions",
                columns: new[] { "UserId", "JokeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLogins_Provider_ProviderUserId",
                table: "SocialLogins",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLogins_UserId",
                table: "SocialLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "SocialLogins");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ComedyTexts");

            migrationBuilder.DropTable(
                name: "Jokes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
