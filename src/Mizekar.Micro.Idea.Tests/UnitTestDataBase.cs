using System;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestDataBase
    {
        [Fact]
        public void CreateInMemoryDataBase()
        {
            var options = new DbContextOptionsBuilder<IdeaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new IdeaDbContext(options, new FakedUserResolverService(1), new FakedTeamResolverService(2)))
            {
                var sampleModel1 = new Data.Entities.IdeaInfo()
                {
                    Slug = "Wf212E",
                    IsDraft = true,
                    Details = "",
                    Problem = "",

                    Achievement = "ثمرات و دستاوردهای تستی",

                };
                context.IdeaInfos.Add(sampleModel1);
                context.SaveChanges();
            }
        }


        [Fact]
        public void CreateSqlLiteDataBase()
        {
            var options = new DbContextOptionsBuilder<IdeaDbContext>()
                .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
                .Options;

            using (var context = new IdeaDbContext(options, new FakedUserResolverService(1), new FakedTeamResolverService(2)))
            {
                context.Database.EnsureCreated();

                var status = new IdeaStatus() { Order = 1, Title = "انتشار اولیه" };
                var sampleModel1 = new Data.Entities.IdeaInfo()
                {
                    Slug = "Wf212E",
                    IsDraft = true,
                    IdeaStatus = status,

                    Details = "",
                    Problem = "",
                    Achievement = "ثمرات و دستاوردهای تستی",

                };
                context.IdeaInfos.Add(sampleModel1);
                context.SaveChanges();


            }
        }

    }
}
