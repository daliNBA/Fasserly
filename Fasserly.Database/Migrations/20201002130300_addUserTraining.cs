using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class addUserTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrainingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Trainings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTrainings",
                columns: table => new
                {
                    UserFasserlyId = table.Column<string>(nullable: false),
                    TrainingId = table.Column<Guid>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    IsHost = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainings", x => new { x.UserFasserlyId, x.TrainingId });
                    table.ForeignKey(
                        name: "FK_UserTrainings_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainings_AspNetUsers_UserFasserlyId",
                        column: x => x.UserFasserlyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_OwnerId",
                table: "Trainings",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainings_TrainingId",
                table: "UserTrainings",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_AspNetUsers_OwnerId",
                table: "Trainings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_AspNetUsers_OwnerId",
                table: "Trainings");

            migrationBuilder.DropTable(
                name: "UserTrainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_OwnerId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Trainings");

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrainingId",
                table: "AspNetUsers",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trainings_TrainingId",
                table: "AspNetUsers",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
