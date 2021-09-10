using Microsoft.EntityFrameworkCore.Migrations;

namespace CodingMilitia.PlayBall.GroupManagement.Data.Migrations
{
    public partial class AddGroupDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Groups");
        }
    }
}
