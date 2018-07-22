using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class AddServiceAndAnnModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnnouncementId",
                table: "IdeaInfos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "IdeaInfos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageId = table.Column<Guid>(nullable: true),
                    IsSpecial = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
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
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageId = table.Column<Guid>(nullable: true),
                    IsSpecial = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
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
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaInfos_AnnouncementId",
                table: "IdeaInfos",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaInfos_ServiceId",
                table: "IdeaInfos",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdeaInfos_Announcements_AnnouncementId",
                table: "IdeaInfos",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IdeaInfos_Services_ServiceId",
                table: "IdeaInfos",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdeaInfos_Announcements_AnnouncementId",
                table: "IdeaInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_IdeaInfos_Services_ServiceId",
                table: "IdeaInfos");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_IdeaInfos_AnnouncementId",
                table: "IdeaInfos");

            migrationBuilder.DropIndex(
                name: "IX_IdeaInfos_ServiceId",
                table: "IdeaInfos");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "IdeaInfos");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "IdeaInfos");
        }
    }
}
