using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Toci.Haia.Database.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "GroupPostId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(770), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(772), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(774), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(775), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(777), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(778), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(779), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(780), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(782), 0 });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CommentTimestamp", "GroupPostId" },
                values: new object[] { new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(783), 0 });

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(982));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 11, 21, 41, 35, 438, DateTimeKind.Utc).AddTicks(991));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GroupPostId",
                table: "Comments",
                column: "GroupPostId");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_GroupPostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_GroupPostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "GroupPostId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_1", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8620) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_2", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8622) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_3", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8624) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_4", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8626) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_5", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8628) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_6", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8630) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_7", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8632) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_8", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8634) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_9", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8636) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Author", "CommentTimestamp" },
                values: new object[] { "Commenter_10", new DateTime(2024, 11, 11, 19, 29, 6, 95, DateTimeKind.Utc).AddTicks(8639) });

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
        }
    }
}
