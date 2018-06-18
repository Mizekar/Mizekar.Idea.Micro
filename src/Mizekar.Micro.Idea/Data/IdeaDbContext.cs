using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Micro.Idea.Data.Entities;

namespace Mizekar.Micro.Idea.Data
{
    public class IdeaDbContext : MizekarBaseDbContext
    {
        public IdeaDbContext()
        {

        }

        public IdeaDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<IdeaInfo> IdeaInfos { get; set; }
        public DbSet<IdeaStatus> IdeaStatuses { get; set; }

        public DbSet<SimilarIdea> SimilarIdeas { get; set; }
        public DbSet<OperationalPhase> OperationalPhases { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Requirement> Requirements { get; set; }

        public DbSet<IdeaOptionSelection> IdeaOptionSelections { get; set; }
        public DbSet<IdeaSocialStatistic> IdeaSocialStatistics { get; set; }

        public DbSet<IdeaOptionSet> IOptionSets { get; set; }
        public DbSet<IdeaOptionSetItem> IdeaOptionSetItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
