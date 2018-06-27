﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mizekar.Micro.Idea.Data;

namespace Mizekar.Micro.Idea.Migrations
{
    [DbContext(typeof(IdeaDbContext))]
    partial class IdeaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.DepartmentLink", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("DepartmentId");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("DepartmentLinks");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Achievement");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Details");

                    b.Property<Guid>("IdeaStatusId");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsDraft");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<string>("Necessity");

                    b.Property<long>("OwnerId");

                    b.Property<int?>("PriorityByOwner");

                    b.Property<string>("Problem");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Slug");

                    b.Property<long>("TeamId");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("IdeaStatusId");

                    b.ToTable("IdeaInfos");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSelection", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("IdeaId");

                    b.Property<Guid>("IdeaOptionSetId");

                    b.Property<Guid>("IdeaOptionSetItemId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.HasIndex("IdeaOptionSetId");

                    b.HasIndex("IdeaOptionSetItemId");

                    b.ToTable("IdeaOptionSelections");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSet", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Category");

                    b.Property<string>("Code");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsMultiSelect");

                    b.Property<bool>("IsSystemField");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<int>("Order");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("IdeaOptionSets");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSetItem", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("HexColor");

                    b.Property<Guid>("IdeaOptionSetId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsDisabled");

                    b.Property<bool>("IsSystemField");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<int>("Order");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.Property<string>("Title");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("IdeaOptionSetId");

                    b.ToTable("IdeaOptionSetItems");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaSocialStatistic", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CommentCount");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("LikeCount");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("ScoreSum");

                    b.Property<long>("TeamId");

                    b.Property<long>("ViewCount");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("IdeaSocialStatistics");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaStatus", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("HexColor");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<int>("Order");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.Property<string>("Title");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("IdeaStatuses");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.OperationalPhase", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<long>("MoneyRequired");

                    b.Property<int>("Order");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.Property<int>("TimeRequiredByDays");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("OperationalPhases");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.Participation", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Expectation");

                    b.Property<string>("FullName");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<string>("PartnershipRate");

                    b.Property<string>("PartnershipStyle");

                    b.Property<string>("PartnershipType");

                    b.Property<string>("Phone");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("ScopeOfPartnership");

                    b.Property<long>("TeamId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("Participations");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.Requirement", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<long>("MoneyRequired");

                    b.Property<int>("Order");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<long>("TeamId");

                    b.Property<int>("TimeRequiredByDays");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("Requirements");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.ScopeLink", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid>("ScopeId");

                    b.Property<long>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("ScopeLinks");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.SimilarIdea", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid?>("CityId");

                    b.Property<Guid>("CountryId");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<Guid>("IdeaId");

                    b.Property<string>("IdeaTitle");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<string>("OrganizationName");

                    b.Property<string>("OwnerFullName");

                    b.Property<string>("Phone");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid?>("StateId");

                    b.Property<long>("TeamId");

                    b.Property<Guid?>("VillageId");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("SimilarIdeas");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.StrategyLink", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid>("StrategyId");

                    b.Property<long>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("StrategyLinks");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.SubjectLink", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<long>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("IdeaId");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("ModifiedById");

                    b.Property<DateTimeOffset?>("ModifiedOn");

                    b.Property<Guid>("RowGuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid>("SubjectId");

                    b.Property<long>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("SubjectLinks");
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.DepartmentLink", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("DepartmentLinks")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaStatus", "IdeaStatus")
                        .WithMany("Ideas")
                        .HasForeignKey("IdeaStatusId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSelection", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("IdeaOptionSelections")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSet", "IdeaOptionSet")
                        .WithMany()
                        .HasForeignKey("IdeaOptionSetId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSetItem", "IdeaOptionSetItem")
                        .WithMany("IdeaInfoOptionSetRelations")
                        .HasForeignKey("IdeaOptionSetItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSetItem", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaOptionSet", "IdeaOptionSet")
                        .WithMany("IdeaOptionSetItems")
                        .HasForeignKey("IdeaOptionSetId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.IdeaSocialStatistic", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("SocialStatistics")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.OperationalPhase", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("OperationalPhases")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.Participation", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("Participations")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.Requirement", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("Requirements")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.ScopeLink", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("ScopeLinks")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.SimilarIdea", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("SimilarIdeas")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.StrategyLink", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("StrategyLinks")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Mizekar.Micro.Idea.Data.Entities.SubjectLink", b =>
                {
                    b.HasOne("Mizekar.Micro.Idea.Data.Entities.IdeaInfo", "Idea")
                        .WithMany("SubjectLinks")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
