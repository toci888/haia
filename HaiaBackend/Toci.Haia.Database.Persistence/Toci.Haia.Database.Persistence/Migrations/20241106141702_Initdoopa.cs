using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initdoopa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jokes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReactionType = table.Column<string>(type: "text", nullable: false),
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


            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "integer",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JokeId",
                table: "Comments",
                type: "integer",
                nullable: true,
                defaultValue: 0);

            
           
            migrationBuilder.CreateIndex(
                name: "IX_Comments_JokeId",
                table: "Comments",
                column: "JokeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_JokeId",
                table: "Reactions",
                column: "JokeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comments_Jokes_JokeId",
            //    table: "Comments",
            //    column: "JokeId",
            //    principalTable: "Jokes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comments_Users_UserId",
            //    table: "Comments",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Jokes_JokeId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Jokes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_JokeId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "JokeId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8893));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8894));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8897));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "ComedyTexts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentTimestamp",
                value: new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8933), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8933), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8937), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8937), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8948), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8948), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8950), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8950), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8952), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8953), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8955), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8955), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8957), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8957), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8959), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8960), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8962), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8962), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CommentTimestamp", "SnippetTimestamp", "UserId" },
                values: new object[] { new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8965), new DateTime(2024, 9, 29, 14, 58, 41, 848, DateTimeKind.Utc).AddTicks(8965), null });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
