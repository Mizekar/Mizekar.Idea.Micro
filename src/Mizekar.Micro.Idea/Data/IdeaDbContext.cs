using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Core.Data.Services;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Data.Entities.Functional;

namespace Mizekar.Micro.Idea.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class IdeaDbContext : MizekarBaseDbContext
    {
        public IdeaDbContext()
        {

        }

        public IdeaDbContext(DbContextOptions options, IUserResolverService userResolverService, ITeamResolverService teamResolverService)
            : base(options, userResolverService, teamResolverService)
        {

        }

        public DbSet<IdeaInfo> IdeaInfos { get; set; }
        public DbSet<IdeaStatus> IdeaStatuses { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SimilarIdea> SimilarIdeas { get; set; }
        public DbSet<OperationalPhase> OperationalPhases { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<IdeaSocialStatistic> IdeaSocialStatistics { get; set; }
        public DbSet<IdeaOptionSelection> IdeaOptionSelections { get; set; }
        public DbSet<IdeaOptionSet> IdeaOptionSets { get; set; }
        public DbSet<IdeaOptionSetItem> IdeaOptionSetItems { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public DbSet<DepartmentLink> DepartmentLinks { get; set; }
        public DbSet<StrategyLink> StrategyLinks { get; set; }
        public DbSet<SubjectLink> SubjectLinks { get; set; }
        public DbSet<ScopeLink> ScopeLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
