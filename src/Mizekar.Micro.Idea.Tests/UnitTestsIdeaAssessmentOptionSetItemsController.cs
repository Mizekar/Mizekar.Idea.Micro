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
    public class UnitTestsIdeaAssessmentOptionSetItemsController
    {
        private readonly IdeaAssessmentOptionSetsController _ideaAssessmentOptionSetsController;
        private readonly IdeaAssessmentOptionSetItemsController _ideaAssessmentOptionSetItemsController;

        public UnitTestsIdeaAssessmentOptionSetItemsController()
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
            _ideaAssessmentOptionSetItemsController = new IdeaAssessmentOptionSetItemsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaAssessmentOptionSetItem()
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


            var ideaAssessmentOptionSetItemPoco = new IdeaAssessmentOptionSetItemPoco()
            {
                Order = 1,
                Title = "عنوان",
                Value = 5,
                IdeaAssessmentOptionSetId = ideaAssessmentOptionSetId
            };
            var ideaAssessmentOptionSetItemResult = await _ideaAssessmentOptionSetItemsController.PostIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemPoco);
            Assert.NotNull(ideaAssessmentOptionSetItemResult);
            Assert.NotNull(ideaAssessmentOptionSetItemResult.Result);
            var ideaAssessmentOptionSetItemResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetItemResult.Result);
            Assert.NotEqual(ideaAssessmentOptionSetItemResultObject.Value, Guid.Empty);
            var ideaAssessmentOptionSetItemId = Assert.IsType<Guid>(ideaAssessmentOptionSetItemResultObject.Value);


            // view
            var ideaAssessmentOptionSetItemViewResult = await _ideaAssessmentOptionSetItemsController.GetIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemId);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult.Result);
            var ideaAssessmentOptionSetItemViewResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetItemViewResult.Result);
            var ideaAssessmentOptionSetItemViewPocoObject = Assert.IsType<IdeaAssessmentOptionSetItemViewPoco>(ideaAssessmentOptionSetItemViewResultObject.Value);
            Assert.Equal(ideaAssessmentOptionSetItemViewPocoObject.Id, ideaAssessmentOptionSetItemId);
            Assert.Equal(ideaAssessmentOptionSetItemViewPocoObject.IdeaAssessmentOptionSetItem.IdeaAssessmentOptionSetId, ideaAssessmentOptionSetId);
            Assert.NotNull(ideaAssessmentOptionSetItemViewPocoObject.IdeaAssessmentOptionSetItem);

            // update
            var ideaAssessmentOptionSetItemTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaAssessmentOptionSetItemPoco.Title = ideaAssessmentOptionSetItemTitle;
            var ideaAssessmentOptionSetItemUpdateResult = await _ideaAssessmentOptionSetItemsController.PutIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemId, ideaAssessmentOptionSetItemPoco);
            Assert.NotNull(ideaAssessmentOptionSetItemUpdateResult);
            Assert.NotNull(ideaAssessmentOptionSetItemUpdateResult.Result);
            var ideaAssessmentOptionSetItemUpdateResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetItemUpdateResult.Result);
            Assert.Equal(ideaAssessmentOptionSetItemUpdateResultObject.Value, ideaAssessmentOptionSetItemId);

            // re check
            var ideaAssessmentOptionSetItemViewResult2 = await _ideaAssessmentOptionSetItemsController.GetIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemId);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult2);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult2.Result);
            var ideaAssessmentOptionSetItemViewResultObject2 = Assert.IsType<OkObjectResult>(ideaAssessmentOptionSetItemViewResult2.Result);
            var ideaAssessmentOptionSetItemViewPocoObject2 = Assert.IsType<IdeaAssessmentOptionSetItemViewPoco>(ideaAssessmentOptionSetItemViewResultObject2.Value);
            Assert.Equal(ideaAssessmentOptionSetItemViewPocoObject2.Id, ideaAssessmentOptionSetItemId);
            Assert.Equal(ideaAssessmentOptionSetItemViewPocoObject2.IdeaAssessmentOptionSetItem.Title, ideaAssessmentOptionSetItemTitle);

            // delete
            var deleteResult = await _ideaAssessmentOptionSetItemsController.DeleteIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaAssessmentOptionSetItemId);


            // view 
            var ideaAssessmentOptionSetItemViewResult3 = await _ideaAssessmentOptionSetItemsController.GetIdeaAssessmentOptionSetItem(ideaAssessmentOptionSetItemId);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult3);
            Assert.NotNull(ideaAssessmentOptionSetItemViewResult3.Result);
            var ideaAssessmentOptionSetItemViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaAssessmentOptionSetItemViewResult3.Result);
            Assert.Equal(ideaAssessmentOptionSetItemViewPocoObject3.Value, ideaAssessmentOptionSetItemId);

        }
    }
}
