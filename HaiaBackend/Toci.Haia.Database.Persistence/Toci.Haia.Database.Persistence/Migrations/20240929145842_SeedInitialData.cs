using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
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

            migrationBuilder.InsertData(
                table: "ComedyTexts",
                columns: new[] { "Id", "Author", "ChildTextId", "CommentTimestamp", "ParentTextId", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, "Author_1", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8886), null, "Sample Comedy Text 1", null },
                    { 2, "Author_2", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8891), null, "Sample Comedy Text 2", null },
                    { 3, "Author_3", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8892), null, "Sample Comedy Text 3", null },
                    { 4, "Author_4", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8893), null, "Sample Comedy Text 4", null },
                    { 5, "Author_5", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8894), null, "Sample Comedy Text 5", null },
                    { 6, "Author_6", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8896), null, "Sample Comedy Text 6", null },
                    { 7, "Author_7", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8897), null, "Sample Comedy Text 7", null },
                    { 8, "Author_8", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8898), null, "Sample Comedy Text 8", null },
                    { 9, "Author_9", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8899), null, "Sample Comedy Text 9", null },
                    { 10, "Author_10", null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8900), null, "Sample Comedy Text 10", null }
                });

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
                columns: new[] { "Id", "Email", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, "user1@example.com", "hash_user_1", "User_1" },
                    { 2, "user2@example.com", "hash_user_2", "User_2" },
                    { 3, "user3@example.com", "hash_user_3", "User_3" },
                    { 4, "user4@example.com", "hash_user_4", "User_4" },
                    { 5, "user5@example.com", "hash_user_5", "User_5" },
                    { 6, "user6@example.com", "hash_user_6", "User_6" },
                    { 7, "user7@example.com", "hash_user_7", "User_7" },
                    { 8, "user8@example.com", "hash_user_8", "User_8" },
                    { 9, "user9@example.com", "hash_user_9", "User_9" },
                    { 10, "user10@example.com", "hash_user_10", "User_10" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Author", "ComedyTextId", "ComedyTextId1", "CommentTimestamp", "GptJoke", "ParentCommentId", "Snippet", "SnippetAuthor", "SnippetTimestamp", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, "Commenter_1", 1, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8933), "Generated Joke 1", null, "Snippet 1", "SnippetAuthor_1", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8933), "Sample Comment 1", null },
                    { 2, "Commenter_2", 2, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8937), "Generated Joke 2", null, "Snippet 2", "SnippetAuthor_2", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8937), "Sample Comment 2", null },
                    { 3, "Commenter_3", 3, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8948), "Generated Joke 3", null, "Snippet 3", "SnippetAuthor_3", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8948), "Sample Comment 3", null },
                    { 4, "Commenter_4", 4, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8950), "Generated Joke 4", null, "Snippet 4", "SnippetAuthor_4", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8950), "Sample Comment 4", null },
                    { 5, "Commenter_5", 5, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8952), "Generated Joke 5", null, "Snippet 5", "SnippetAuthor_5", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8953), "Sample Comment 5", null },
                    { 6, "Commenter_6", 6, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8955), "Generated Joke 6", null, "Snippet 6", "SnippetAuthor_6", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8955), "Sample Comment 6", null },
                    { 7, "Commenter_7", 7, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8957), "Generated Joke 7", null, "Snippet 7", "SnippetAuthor_7", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8957), "Sample Comment 7", null },
                    { 8, "Commenter_8", 8, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8959), "Generated Joke 8", null, "Snippet 8", "SnippetAuthor_8", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8960), "Sample Comment 8", null },
                    { 9, "Commenter_9", 9, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8962), "Generated Joke 9", null, "Snippet 9", "SnippetAuthor_9", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8962), "Sample Comment 9", null },
                    { 10, "Commenter_10", 10, null, new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8965), "Generated Joke 10", null, "Snippet 10", "SnippetAuthor_10", new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8965), "Sample Comment 10", null }
                });

            migrationBuilder.InsertData(
                table: "Friendships",
                columns: new[] { "Id", "FriendId", "UserId" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 3, 2 },
                    { 3, 4, 3 },
                    { 4, 5, 4 },
                    { 5, 6, 5 },
                    { 6, 7, 6 },
                    { 7, 8, 7 },
                    { 8, 9, 8 },
                    { 9, 10, 9 },
                    { 10, 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "Id", "CommentId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 4 },
                    { 5, 5, 5 },
                    { 6, 6, 6 },
                    { 7, 7, 7 },
                    { 8, 8, 8 },
                    { 9, 9, 9 },
                    { 10, 10, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Friendships",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
