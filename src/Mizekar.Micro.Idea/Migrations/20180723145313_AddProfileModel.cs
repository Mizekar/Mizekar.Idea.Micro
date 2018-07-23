using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class AddProfileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsExpertUser = table.Column<bool>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
