using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models.IdeaAssessmentOptions;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsIdeaAssessmentOptionSetsController
    {
        private readonly IdeaAssessmentOptionSetsController _ideaAssessmentOptionSetsController;

        public UnitTestsIdeaAssessmentOptionSetsController()
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
            _ideaAssessmentOptionSetsController = new IdeaAssessmentOptionSetsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaAssessmentOptionSet()
        {
            var ideaAssessmentOptionSetPoco = new IdeaAssessmentOptionSetPoco()
            {
                Order = 1,
                Title = "عنوان",
                Description = "توضیحات",
            };
            var ideaAssessmentOptionSetResult = await _ideaAssessmentOptionSetsController.PostIdeaAssessmentOptionSet(ideaAssessmentOptionSetPoco);
            Assert.NotNull(ideaAssessmentOptionSetResult);
            Assert.NotNull(ideaAssessmentOptionSetResult.Result);
            var ideaAssessmentOptionSetResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetResult.Result);
            Assert.NotEqual(ideaAssessmentOptionSetResultObject.Value, Guid.Empty);
            var ideaAssessmentOptionSetId = Assert.IsType<Guid>(ideaAssessmentOptionSetResultObject.Value);


            // view
            var ideaAssessmentOptionSetViewResult = await _ideaAssessmentOptionSetsController.GetIdeaAssessmentOptionSet(ideaAssessmentOptionSetId);
            Assert.NotNull(ideaAssessmentOptionSetViewResult);
            Assert.NotNull(ideaAssessmentOptionSetViewResult.Result);
            var ideaAssessmentOptionSetViewResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetViewResult.Result);
            var ideaAssessmentOptionSetViewPocoObject = Assert.IsType<IdeaAssessmentOptionSetViewPoco>(ideaAssessmentOptionSetViewResultObject.Value);
            Assert.Equal(ideaAssessmentOptionSetViewPocoObject.Id, ideaAssessmentOptionSetId);
            Assert.NotNull(ideaAssessmentOptionSetViewPocoObject.IdeaAssessmentOptionSet);

            // update
            var ideaAssessmentOptionSetTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaAssessmentOptionSetPoco.Title = ideaAssessmentOptionSetTitle;
            var ideaAssessmentOptionSetUpdateResult = await _ideaAssessmentOptionSetsController.PutIdeaAssessmentOptionSet(ideaAssessmentOptionSetId, ideaAssessmentOptionSetPoco);
            Assert.NotNull(ideaAssessmentOptionSetUpdateResult);
            Assert.NotNull(ideaAssessmentOptionSetUpdateResult.Result);
            var ideaAssessmentOptionSetUpdateResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetUpdateResult.Result);
            Assert.Equal(ideaAssessmentOptionSetUpdateResultObject.Value, ideaAssessmentOptionSetId);

            // re check
            var ideaAssessmentOptionSetViewResult2 = await _ideaAssessmentOptionSetsController.GetIdeaAssessmentOptionSet(ideaAssessmentOptionSetId);
            Assert.NotNull(ideaAssessmentOptionSetViewResult2);
            Assert.NotNull(ideaAssessmentOptionSetViewResult2.Result);
            var ideaAssessmentOptionSetViewResultObject2 = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetViewResult2.Result);
            var ideaAssessmentOptionSetViewPocoObject2 = Assert.IsType<IdeaAssessmentOptionSetViewPoco>(ideaAssessmentOptionSetViewResultObject2.Value);
            Assert.Equal(ideaAssessmentOptionSetViewPocoObject2.Id, ideaAssessmentOptionSetId);
            Assert.Equal(ideaAssessmentOptionSetViewPocoObject2.IdeaAssessmentOptionSet.Title, ideaAssessmentOptionSetTitle);

            // delete
            var deleteResult = await _ideaAssessmentOptionSetsController.DeleteIdeaAssessmentOptionSet(ideaAssessmentOptionSetId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaAssessmentOptionSetId);


            // view 
            var ideaAssessmentOptionSetViewResult3 = await _ideaAssessmentOptionSetsController.GetIdeaAssessmentOptionSet(ideaAssessmentOptionSetId);
            Assert.NotNull(ideaAssessmentOptionSetViewResult3);
            Assert.NotNull(ideaAssessmentOptionSetViewResult3.Result);
            var ideaAssessmentOptionSetViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaAssessmentOptionSetViewResult3.Result);
            Assert.Equal(ideaAssessmentOptionSetViewPocoObject3.Value, ideaAssessmentOptionSetId);

        }
    }
}
