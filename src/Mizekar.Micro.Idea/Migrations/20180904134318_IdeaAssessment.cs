using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class IdeaAssessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Value",
                table: "IdeaOptionSetItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IdeaAssessmentOptionSets",
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
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    IsMultiSelect = table.Column<bool>(nullable: false),
                    IsSystemField = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaAssessmentOptionSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdeaAssessmentScores",
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
                    IdeaId = table.Column<Guid>(nullable: false),
                    Score = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaAssessmentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaAssessmentScores_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdeaAssessmentOptionSetItems",
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
                    IdeaAssessmentOptionSetId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<long>(nullable: false),
                    HexColor = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    IsSystemField = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaAssessmentOptionSetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaAssessmentOptionSetItems_IdeaAssessmentOptionSets_IdeaAssessmentOptionSetId",
                        column: x => x.IdeaAssessmentOptionSetId,
                        principalTable: "IdeaAssessmentOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdeaAssessmentOptionSelections",
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
                    IdeaId = table.Column<Guid>(nullable: false),
                    IdeaAssessmentOptionSetId = table.Column<Guid>(nullable: false),
                    IdeaAssessmentOptionSetItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaAssessmentOptionSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaAssessmentOptionSelections_IdeaAssessmentOptionSets_IdeaAssessmentOptionSetId",
                        column: x => x.IdeaAssessmentOptionSetId,
                        principalTable: "IdeaAssessmentOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdeaAssessmentOptionSelections_IdeaAssessmentOptionSetItems_IdeaAssessmentOptionSetItemId",
                        column: x => x.IdeaAssessmentOptionSetItemId,
                        principalTable: "IdeaAssessmentOptionSetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdeaAssessmentOptionSelections_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaAssessmentOptionSelections_IdeaAssessmentOptionSetId",
                table: "IdeaAssessmentOptionSelections",
                column: "IdeaAssessmentOptionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaAssessmentOptionSelections_IdeaAssessmentOptionSetItemId",
                table: "IdeaAssessmentOptionSelections",
                column: "IdeaAssessmentOptionSetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaAssessmentOptionSelections_IdeaId",
                table: "IdeaAssessmentOptionSelections",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaAssessmentOptionSetItems_IdeaAssessmentOptionSetId",
                table: "IdeaAssessmentOptionSetItems",
                column: "IdeaAssessmentOptionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaAssessmentScores_IdeaId",
                table: "IdeaAssessmentScores",
                column: "IdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdeaAssessmentOptionSelections");

            migrationBuilder.DropTable(
                name: "IdeaAssessmentScores");

            migrationBuilder.DropTable(
                name: "IdeaAssessmentOptionSetItems");

            migrationBuilder.DropTable(
                name: "IdeaAssessmentOptionSets");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdeaOptionSetItems",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
