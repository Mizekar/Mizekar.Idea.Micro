//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Mizekar.Micro.Idea.Controllers;
//using Mizekar.Micro.Idea.Data;
//using Microsoft.AspNetCore.Mvc;
//using Mizekar.Micro.Idea.Data.Entities;
//using Xunit;

//namespace Mizekar.Micro.Idea.Tests
//{
//    public class IdeasController_Tests
//    {
//        private IdeaDbContext _ideaDbContext;
//        private IdeasController _ideasController;

//        public IdeasController_Tests()
//        {
//            var options = new DbContextOptionsBuilder<IdeaDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            _ideaDbContext = new IdeaDbContext(options);
//            SeedData(_ideaDbContext);
//            _ideasController = new IdeasController(_ideaDbContext);
//        }

//        private static void SeedData(IdeaDbContext ideaDbContext)
//        {
//            var idea1 = new IdeaInfo()
//            {
//                Slug = "Wf212E",
//                IsDraft = true,

//                Subject = "موضوع تستی",
//                Summary = "",
//                Details = "",
//                Problem = "",
//                Priority = 1,
//                IsPrivate = true,

//                Achievement = "ثمرات و دستاوردهای تستی",
//            };
//            ideaDbContext.IdeaInfos.Add(idea1);
//        }

//        [Fact]
//        public void GetAll()
//        {
//            var result = _ideasController.Get();
//            Assert.NotNull(result);
//            Assert.NotNull(result);
//        }
//    }
//}
