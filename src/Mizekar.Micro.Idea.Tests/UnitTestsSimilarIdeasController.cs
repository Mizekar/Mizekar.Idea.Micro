using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Similar;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsSimilarIdeasController
    {
        private readonly SimilarIdeasController _similarIdeasController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsSimilarIdeasController()
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

            _ideasController = new IdeasController(context, imapper, fakedUserResolverService);
            _ideaStatusesController = new IdeaStatusesController(context, imapper);
            _similarIdeasController = new SimilarIdeasController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudSimilarIdea()
        {
            var status = new IdeaStatusPoco() { Order = 1, Title = "انتشار اولیه" };
            var statusResult = await _ideaStatusesController.PostIdeaStatus(status);

            Assert.NotNull(statusResult);
            Assert.NotNull(statusResult.Result);
            var statusResultObject = Assert.IsType<OkObjectResult>(statusResult.Result);
            Assert.NotEqual(statusResultObject.Value, Guid.Empty);
            var statusId = Assert.IsType<Guid>(statusResultObject.Value);

            var userId = 1;
            var ideaPoco = new Models.IdeaPoco()
            {
                Text = "ایده من",
                Title = "عنوان ایده",
                IsDraft = false,
                OwnerId = userId,
                IdeaStatusId = statusId,
                PriorityByOwner = 5
            };
            var ideaResult = await _ideasController.PostIdea(ideaPoco);
            Assert.NotNull(ideaResult);
            Assert.NotNull(ideaResult.Result);
            var ideaResultObject = Assert.IsType<OkObjectResult>(ideaResult.Result);
            Assert.NotEqual(ideaResultObject.Value, Guid.Empty);
            var ideaId = Assert.IsType<Guid>(ideaResultObject.Value);


            var similarIdeaPoco = new SimilarIdeaPoco()
            {
                IdeaId = ideaId,
                IdeaTitle = "عنوان",
                CountryId = Guid.NewGuid(),
                Description = "توضیحات",
            };
            var similarIdeaResult = await _similarIdeasController.PostSimilarIdea(similarIdeaPoco);
            Assert.NotNull(similarIdeaResult);
            Assert.NotNull(similarIdeaResult.Result);
            var similarIdeaResultObject = Assert.IsType<OkObjectResult>(similarIdeaResult.Result);
            Assert.NotEqual(similarIdeaResultObject.Value, Guid.Empty);
            var similarIdeaId = Assert.IsType<Guid>(similarIdeaResultObject.Value);


            // view
            var similarIdeaViewResult = await _similarIdeasController.GetSimilarIdeaInfo(similarIdeaId);
            Assert.NotNull(similarIdeaViewResult);
            Assert.NotNull(similarIdeaViewResult.Result);
            var similarIdeaViewResultObject = Assert.IsType<OkObjectResult>(similarIdeaViewResult.Result);
            var similarIdeaViewPocoObject = Assert.IsType<SimilarIdeaViewPoco>(similarIdeaViewResultObject.Value);
            Assert.Equal(similarIdeaViewPocoObject.Id, similarIdeaId);
            Assert.NotNull(similarIdeaViewPocoObject.SimilarIdea);

            // update
            var similarIdeaTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            similarIdeaPoco.Description = similarIdeaTitle;
            var similarIdeaUpdateResult = await _similarIdeasController.PutSimilarIdeaInfo(similarIdeaId, similarIdeaPoco);
            Assert.NotNull(similarIdeaUpdateResult);
            Assert.NotNull(similarIdeaUpdateResult.Result);
            var similarIdeaUpdateResultObject = Assert.IsType<OkObjectResult>(similarIdeaUpdateResult.Result);
            Assert.Equal(similarIdeaUpdateResultObject.Value, similarIdeaId);

            // re check
            var similarIdeaViewResult2 = await _similarIdeasController.GetSimilarIdeaInfo(similarIdeaId);
            Assert.NotNull(similarIdeaViewResult2);
            Assert.NotNull(similarIdeaViewResult2.Result);
            var similarIdeaViewResultObject2 = Assert.IsType<OkObjectResult>(similarIdeaViewResult2.Result);
            var similarIdeaViewPocoObject2 = Assert.IsType<SimilarIdeaViewPoco>(similarIdeaViewResultObject2.Value);
            Assert.Equal(similarIdeaViewPocoObject2.Id, similarIdeaId);
            Assert.Equal(similarIdeaViewPocoObject2.SimilarIdea.Description, similarIdeaTitle);

            // delete
            var deleteResult = await _similarIdeasController.DeleteSimilarIdea(similarIdeaId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, similarIdeaId);


            // view 
            var similarIdeaViewResult3 = await _similarIdeasController.GetSimilarIdeaInfo(similarIdeaId);
            Assert.NotNull(similarIdeaViewResult3);
            Assert.NotNull(similarIdeaViewResult3.Result);
            var similarIdeaViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(similarIdeaViewResult3.Result);
            Assert.Equal(similarIdeaViewPocoObject3.Value, similarIdeaId);

        }
    }
}
