using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class AddOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_AspNetUsers_OwnerId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_OwnerId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Trainings");

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "UserTrainings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserFasserlyId",
                table: "Trainings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_UserFasserlyId",
                table: "Trainings",
                column: "UserFasserlyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_AspNetUsers_UserFasserlyId",
                table: "Trainings",
                column: "UserFasserlyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_AspNetUsers_UserFasserlyId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_UserFasserlyId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "UserTrainings");

            migrationBuilder.DropColumn(
                name: "UserFasserlyId",
                table: "Trainings");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Trainings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_OwnerId",
                table: "Trainings",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_AspNetUsers_OwnerId",
                table: "Trainings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
