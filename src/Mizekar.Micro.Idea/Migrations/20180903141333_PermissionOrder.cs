using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class PermissionOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Permissions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Permissions");
        }
    }
}
