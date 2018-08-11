using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class GeoFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "IdeaInfos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "IdeaInfos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "IdeaInfos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VillageId",
                table: "IdeaInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "IdeaInfos");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "IdeaInfos");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "IdeaInfos");

            migrationBuilder.DropColumn(
                name: "VillageId",
                table: "IdeaInfos");
        }
    }
}
