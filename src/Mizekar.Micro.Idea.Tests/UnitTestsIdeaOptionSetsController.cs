using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models.IdeaOptions;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsIdeaOptionSetsController
    {
        private readonly IdeaOptionSetsController _ideaOptionSetsController;

        public UnitTestsIdeaOptionSetsController()
        {
            var fakedUserResolverService = new FakedUserResolverService(1);
            var fakedTeamResolverService = new FakedTeamResolverService(1);
            var context = new IdeaDbContext(DbOptionsSqlite, fakedUserResolverService, fakedTeamResolverService);
            context.Database.EnsureCreated();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PublicMapper());
            });
            var imapper = mockMapper.CreateMapper();
            _ideaOptionSetsController = new IdeaOptionSetsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaOptionSet()
        {
            var ideaOptionSetPoco = new IdeaOptionSetPoco()
            {
                Order = 1,
                Title = "عنوان",
                Description = "توضیحات",
            };
            var ideaOptionSetResult = await _ideaOptionSetsController.PostIdeaOptionSet(ideaOptionSetPoco);
            Assert.NotNull(ideaOptionSetResult);
            Assert.NotNull(ideaOptionSetResult.Result);
            var ideaOptionSetResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetResult.Result);
            Assert.NotEqual(ideaOptionSetResultObject.Value, Guid.Empty);
            var ideaOptionSetId = Assert.IsType<Guid>(ideaOptionSetResultObject.Value);


            // view
            var ideaOptionSetViewResult = await _ideaOptionSetsController.GetIdeaOptionSetInfo(ideaOptionSetId);
            Assert.NotNull(ideaOptionSetViewResult);
            Assert.NotNull(ideaOptionSetViewResult.Result);
            var ideaOptionSetViewResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetViewResult.Result);
            var ideaOptionSetViewPocoObject = Assert.IsType<IdeaOptionSetViewPoco>(ideaOptionSetViewResultObject.Value);
            Assert.Equal(ideaOptionSetViewPocoObject.Id, ideaOptionSetId);
            Assert.NotNull(ideaOptionSetViewPocoObject.IdeaOptionSet);

            // update
            var ideaOptionSetTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaOptionSetPoco.Title = ideaOptionSetTitle;
            var ideaOptionSetUpdateResult = await _ideaOptionSetsController.PutIdeaOptionSetInfo(ideaOptionSetId, ideaOptionSetPoco);
            Assert.NotNull(ideaOptionSetUpdateResult);
            Assert.NotNull(ideaOptionSetUpdateResult.Result);
            var ideaOptionSetUpdateResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetUpdateResult.Result);
            Assert.Equal(ideaOptionSetUpdateResultObject.Value, ideaOptionSetId);

            // re check
            var ideaOptionSetViewResult2 = await _ideaOptionSetsController.GetIdeaOptionSetInfo(ideaOptionSetId);
            Assert.NotNull(ideaOptionSetViewResult2);
            Assert.NotNull(ideaOptionSetViewResult2.Result);
            var ideaOptionSetViewResultObject2 = Assert.IsType<OkObjectResult>(ideaOptionSetViewResult2.Result);
            var ideaOptionSetViewPocoObject2 = Assert.IsType<IdeaOptionSetViewPoco>(ideaOptionSetViewResultObject2.Value);
            Assert.Equal(ideaOptionSetViewPocoObject2.Id, ideaOptionSetId);
            Assert.Equal(ideaOptionSetViewPocoObject2.IdeaOptionSet.Title, ideaOptionSetTitle);

            // delete
            var deleteResult = await _ideaOptionSetsController.DeleteIdeaOptionSet(ideaOptionSetId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaOptionSetId);


            // view 
            var ideaOptionSetViewResult3 = await _ideaOptionSetsController.GetIdeaOptionSetInfo(ideaOptionSetId);
            Assert.NotNull(ideaOptionSetViewResult3);
            Assert.NotNull(ideaOptionSetViewResult3.Result);
            var ideaOptionSetViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaOptionSetViewResult3.Result);
            Assert.Equal(ideaOptionSetViewPocoObject3.Value, ideaOptionSetId);

        }
    }
}
