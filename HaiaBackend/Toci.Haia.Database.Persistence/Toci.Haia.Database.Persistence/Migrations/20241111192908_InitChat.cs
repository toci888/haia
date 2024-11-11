using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_UserId",
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
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8518));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8520));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8522));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8540));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8542));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8544));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8549));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8551));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8622));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8624));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8626));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8628));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8630));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8632));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8634));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8639));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 94, DateTimeKind.Utc).AddTicks(3563));

            migrationBuilder.UpdateData(
                table: "Jokes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 94, DateTimeKind.Utc).AddTicks(3568));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 1,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8825));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 2,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8829));

            migrationBuilder.UpdateData(
                table: "SocialLogins",
                keyColumn: "Id",
                keyValue: 3,
                column: "LinkedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8831));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8797));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8801));

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatRoomId",
                table: "ChatMessages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserId",
                table: "ChatMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRooms");

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
        }
    }
}
