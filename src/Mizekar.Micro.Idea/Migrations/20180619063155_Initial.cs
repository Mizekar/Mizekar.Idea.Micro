using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mizekar.Micro.Idea.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdeaStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    HexColor = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_IdeaStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IOptionSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    IsMultiSelect = table.Column<bool>(nullable: false),
                    IsSystemField = table.Column<bool>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOptionSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdeaInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDraft = table.Column<bool>(nullable: false),
                    IdeaStatusId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    PriorityByOwner = table.Column<int>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Achievement = table.Column<string>(nullable: true),
                    Necessity = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Problem = table.Column<string>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaInfos_IdeaStatuses_IdeaStatusId",
                        column: x => x.IdeaStatusId,
                        principalTable: "IdeaStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdeaOptionSetItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaOptionSetId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    HexColor = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    IsSystemField = table.Column<bool>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaOptionSetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaOptionSetItems_IOptionSets_IdeaOptionSetId",
                        column: x => x.IdeaOptionSetId,
                        principalTable: "IOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentLinks_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdeaSocialStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    LikeCount = table.Column<long>(nullable: false),
                    ScoreSum = table.Column<long>(nullable: false),
                    CommentCount = table.Column<long>(nullable: false),
                    ViewCount = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaSocialStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaSocialStatistics_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationalPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    TimeRequiredByDays = table.Column<int>(nullable: false),
                    MoneyRequired = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationalPhases_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PartnershipType = table.Column<string>(nullable: true),
                    PartnershipStyle = table.Column<string>(nullable: true),
                    PartnershipRate = table.Column<string>(nullable: true),
                    ScopeOfPartnership = table.Column<string>(nullable: true),
                    Expectation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participations_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    TimeRequiredByDays = table.Column<int>(nullable: false),
                    MoneyRequired = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requirements_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScopeLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    ScopeId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeLinks_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimilarIdeas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    OwnerFullName = table.Column<string>(nullable: true),
                    IdeaTitle = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: false),
                    StateId = table.Column<Guid>(nullable: true),
                    CityId = table.Column<Guid>(nullable: true),
                    VillageId = table.Column<Guid>(nullable: true),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarIdeas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimilarIdeas_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StrategyLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    StrategyId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrategyLinks_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectLinks_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdeaOptionSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdeaId = table.Column<Guid>(nullable: false),
                    IdeaOptionSetId = table.Column<Guid>(nullable: false),
                    IdeaOptionSetItemId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedById = table.Column<long>(nullable: true),
                    RowGuid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaOptionSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaOptionSelections_IdeaInfos_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "IdeaInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdeaOptionSelections_IOptionSets_IdeaOptionSetId",
                        column: x => x.IdeaOptionSetId,
                        principalTable: "IOptionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdeaOptionSelections_IdeaOptionSetItems_IdeaOptionSetItemId",
                        column: x => x.IdeaOptionSetItemId,
                        principalTable: "IdeaOptionSetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLinks_IdeaId",
                table: "DepartmentLinks",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaInfos_IdeaStatusId",
                table: "IdeaInfos",
                column: "IdeaStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOptionSelections_IdeaId",
                table: "IdeaOptionSelections",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOptionSelections_IdeaOptionSetId",
                table: "IdeaOptionSelections",
                column: "IdeaOptionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOptionSelections_IdeaOptionSetItemId",
                table: "IdeaOptionSelections",
                column: "IdeaOptionSetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOptionSetItems_IdeaOptionSetId",
                table: "IdeaOptionSetItems",
                column: "IdeaOptionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaSocialStatistics_IdeaId",
                table: "IdeaSocialStatistics",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalPhases_IdeaId",
                table: "OperationalPhases",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_IdeaId",
                table: "Participations",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_IdeaId",
                table: "Requirements",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeLinks_IdeaId",
                table: "ScopeLinks",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_SimilarIdeas_IdeaId",
                table: "SimilarIdeas",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyLinks_IdeaId",
                table: "StrategyLinks",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLinks_IdeaId",
                table: "SubjectLinks",
                column: "IdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentLinks");

            migrationBuilder.DropTable(
                name: "IdeaOptionSelections");

            migrationBuilder.DropTable(
                name: "IdeaSocialStatistics");

            migrationBuilder.DropTable(
                name: "OperationalPhases");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.DropTable(
                name: "ScopeLinks");

            migrationBuilder.DropTable(
                name: "SimilarIdeas");

            migrationBuilder.DropTable(
                name: "StrategyLinks");

            migrationBuilder.DropTable(
                name: "SubjectLinks");

            migrationBuilder.DropTable(
                name: "IdeaOptionSetItems");

            migrationBuilder.DropTable(
                name: "IdeaInfos");

            migrationBuilder.DropTable(
                name: "IOptionSets");

            migrationBuilder.DropTable(
                name: "IdeaStatuses");
        }
    }
}
