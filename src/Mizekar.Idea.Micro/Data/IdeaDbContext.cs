using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Idea.Micro.Data.Entities;

namespace Mizekar.Idea.Micro.Data
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
        public DbSet<IdeaAtachement> IdeaAtachements { get; set; }
        public DbSet<ImplementedPastInfo> ImplementedPastInfos { get; set; }
        public DbSet<OperationalPhase> OperationalPhases { get; set; }
        public DbSet<ParticipationInfo> ParticipationInfos { get; set; }
        public DbSet<RequirementEquipments> RequirementEquipments { get; set; }
        public DbSet<OptionSet> OptionSets { get; set; }
        public DbSet<OptionSetItem> OptionSetItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
