using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Languange",
                table: "Trainings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Trainings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Trainings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Trainings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Trainings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Trainings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languange",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
