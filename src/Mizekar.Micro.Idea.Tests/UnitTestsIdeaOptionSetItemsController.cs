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
    public class UnitTestsIdeaOptionSetItemsController
    {
        private readonly IdeaOptionSetsController _ideaOptionSetsController;
        private readonly IdeaOptionSetItemsController _ideaOptionSetItemsController;

        public UnitTestsIdeaOptionSetItemsController()
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
            _ideaOptionSetItemsController = new IdeaOptionSetItemsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaOptionSetItem()
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


            var ideaOptionSetItemPoco = new IdeaOptionSetItemPoco()
            {
                Order = 1,
                Title = "عنوان فاز",
                IdeaOptionSetId = ideaOptionSetId
            };
            var ideaOptionSetItemResult = await _ideaOptionSetItemsController.PostIdeaOptionSetItem(ideaOptionSetItemPoco);
            Assert.NotNull(ideaOptionSetItemResult);
            Assert.NotNull(ideaOptionSetItemResult.Result);
            var ideaOptionSetItemResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetItemResult.Result);
            Assert.NotEqual(ideaOptionSetItemResultObject.Value, Guid.Empty);
            var ideaOptionSetItemId = Assert.IsType<Guid>(ideaOptionSetItemResultObject.Value);


            // view
            var ideaOptionSetItemViewResult = await _ideaOptionSetItemsController.GetIdeaOptionSetItemInfo(ideaOptionSetItemId);
            Assert.NotNull(ideaOptionSetItemViewResult);
            Assert.NotNull(ideaOptionSetItemViewResult.Result);
            var ideaOptionSetItemViewResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetItemViewResult.Result);
            var ideaOptionSetItemViewPocoObject = Assert.IsType<IdeaOptionSetItemViewPoco>(ideaOptionSetItemViewResultObject.Value);
            Assert.Equal(ideaOptionSetItemViewPocoObject.Id, ideaOptionSetItemId);
            Assert.Equal(ideaOptionSetItemViewPocoObject.IdeaOptionSetItem.IdeaOptionSetId, ideaOptionSetId);
            Assert.NotNull(ideaOptionSetItemViewPocoObject.IdeaOptionSetItem);

            // update
            var ideaOptionSetItemTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaOptionSetItemPoco.Title = ideaOptionSetItemTitle;
            var ideaOptionSetItemUpdateResult = await _ideaOptionSetItemsController.PutIdeaOptionSetItemInfo(ideaOptionSetItemId, ideaOptionSetItemPoco);
            Assert.NotNull(ideaOptionSetItemUpdateResult);
            Assert.NotNull(ideaOptionSetItemUpdateResult.Result);
            var ideaOptionSetItemUpdateResultObject = Assert.IsType<OkObjectResult>(ideaOptionSetItemUpdateResult.Result);
            Assert.Equal(ideaOptionSetItemUpdateResultObject.Value, ideaOptionSetItemId);

            // re check
            var ideaOptionSetItemViewResult2 = await _ideaOptionSetItemsController.GetIdeaOptionSetItemInfo(ideaOptionSetItemId);
            Assert.NotNull(ideaOptionSetItemViewResult2);
            Assert.NotNull(ideaOptionSetItemViewResult2.Result);
            var ideaOptionSetItemViewResultObject2 = Assert.IsType<OkObjectResult>(ideaOptionSetItemViewResult2.Result);
            var ideaOptionSetItemViewPocoObject2 = Assert.IsType<IdeaOptionSetItemViewPoco>(ideaOptionSetItemViewResultObject2.Value);
            Assert.Equal(ideaOptionSetItemViewPocoObject2.Id, ideaOptionSetItemId);
            Assert.Equal(ideaOptionSetItemViewPocoObject2.IdeaOptionSetItem.Title, ideaOptionSetItemTitle);

            // delete
            var deleteResult = await _ideaOptionSetItemsController.DeleteIdeaOptionSetItem(ideaOptionSetItemId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaOptionSetItemId);


            // view 
            var ideaOptionSetItemViewResult3 = await _ideaOptionSetItemsController.GetIdeaOptionSetItemInfo(ideaOptionSetItemId);
            Assert.NotNull(ideaOptionSetItemViewResult3);
            Assert.NotNull(ideaOptionSetItemViewResult3.Result);
            var ideaOptionSetItemViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaOptionSetItemViewResult3.Result);
            Assert.Equal(ideaOptionSetItemViewPocoObject3.Value, ideaOptionSetItemId);

        }
    }
}
