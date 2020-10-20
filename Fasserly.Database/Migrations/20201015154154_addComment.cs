using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class addComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserFasserlyId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserFasserlyId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserFasserlyId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserFasserlyId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserFasserlyId",
                table: "Comments",
                column: "UserFasserlyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserFasserlyId",
                table: "Comments",
                column: "UserFasserlyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
