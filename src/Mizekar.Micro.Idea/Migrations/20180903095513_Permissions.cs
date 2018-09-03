using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class Permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionOwners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionOwners_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionOwners_PermissionId",
                table: "PermissionOwners",
                column: "PermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionOwners");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
