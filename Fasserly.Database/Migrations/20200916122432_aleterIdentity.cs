using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class aleterIdentity : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
