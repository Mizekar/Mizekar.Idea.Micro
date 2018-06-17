using System;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Data;
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

            using (var context = new IdeaDbContext(options))
            {
                var sampleModel1 = new Data.Entities.IdeaInfo()
                {
                    Slug = "Wf212E",
                    IsDraft = true,

                    Subject = "موضوع تستی",
                    Summary = "",
                    Details = "",
                    Problem = "",
                    Priority = 1,
                    IsPrivate = true,

                    Achievement = "ثمرات و دستاوردهای تستی",

                };
                context.IdeaInfos.Add(sampleModel1);
                context.SaveChanges();
            }
        }

    }
}
