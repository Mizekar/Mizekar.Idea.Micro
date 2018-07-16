using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsIdeaStatussController
    {
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsIdeaStatussController()
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

            _ideaStatusesController = new IdeaStatusesController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaStatus()
        {
            var ideaStatusPoco = new IdeaStatusPoco()
            {
                Order = 1,
                Title = "title",
            };
            var ideaStatusResult = await _ideaStatusesController.PostIdeaStatus(ideaStatusPoco);
            Assert.NotNull(ideaStatusResult);
            Assert.NotNull(ideaStatusResult.Result);
            var ideaStatusResultObject = Assert.IsType<OkObjectResult>(ideaStatusResult.Result);
            Assert.NotEqual(ideaStatusResultObject.Value, Guid.Empty);
            var ideaStatusId = Assert.IsType<Guid>(ideaStatusResultObject.Value);


            // view
            var ideaStatusViewResult = await _ideaStatusesController.GetIdeaStatusInfo(ideaStatusId);
            Assert.NotNull(ideaStatusViewResult);
            Assert.NotNull(ideaStatusViewResult.Result);
            var ideaStatusViewResultObject = Assert.IsType<OkObjectResult>(ideaStatusViewResult.Result);
            var ideaStatusViewPocoObject = Assert.IsType<IdeaStatusViewPoco>(ideaStatusViewResultObject.Value);
            Assert.Equal(ideaStatusViewPocoObject.Id, ideaStatusId);
            Assert.NotNull(ideaStatusViewPocoObject.IdeaStatus);

            // update
            var ideaStatusTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaStatusPoco.Title = ideaStatusTitle;
            var ideaStatusUpdateResult = await _ideaStatusesController.PutIdeaStatusInfo(ideaStatusId, ideaStatusPoco);
            Assert.NotNull(ideaStatusUpdateResult);
            Assert.NotNull(ideaStatusUpdateResult.Result);
            var ideaStatusUpdateResultObject = Assert.IsType<OkObjectResult>(ideaStatusUpdateResult.Result);
            Assert.Equal(ideaStatusUpdateResultObject.Value, ideaStatusId);

            // re check
            var ideaStatusViewResult2 = await _ideaStatusesController.GetIdeaStatusInfo(ideaStatusId);
            Assert.NotNull(ideaStatusViewResult2);
            Assert.NotNull(ideaStatusViewResult2.Result);
            var ideaStatusViewResultObject2 = Assert.IsType<OkObjectResult>(ideaStatusViewResult2.Result);
            var ideaStatusViewPocoObject2 = Assert.IsType<IdeaStatusViewPoco>(ideaStatusViewResultObject2.Value);
            Assert.Equal(ideaStatusViewPocoObject2.Id, ideaStatusId);
            Assert.Equal(ideaStatusViewPocoObject2.IdeaStatus.Title, ideaStatusTitle);

            // delete
            var deleteResult = await _ideaStatusesController.DeleteIdeaStatus(ideaStatusId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaStatusId);


            // view 
            var ideaStatusViewResult3 = await _ideaStatusesController.GetIdeaStatusInfo(ideaStatusId);
            Assert.NotNull(ideaStatusViewResult3);
            Assert.NotNull(ideaStatusViewResult3.Result);
            var ideaStatusViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaStatusViewResult3.Result);
            Assert.Equal(ideaStatusViewPocoObject3.Value, ideaStatusId);

        }
    }
}
