using Microsoft.EntityFrameworkCore.Migrations;

namespace Fasserly.Database.Migrations
{
    public partial class deleteOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "UserTrainings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "UserTrainings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
