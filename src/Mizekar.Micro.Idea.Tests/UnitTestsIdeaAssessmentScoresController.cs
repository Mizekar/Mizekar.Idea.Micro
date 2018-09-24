using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.IdeaAssessmentOptions;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsIdeaAssessmentScoresController
    {
        private readonly IdeaAssessmentScoresController _ideaAssessmentScoresController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsIdeaAssessmentScoresController()
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
            _ideaAssessmentScoresController = new IdeaAssessmentScoresController(context, imapper, fakedUserResolverService);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdeaAssessmentScore()
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


            var ideaAssessmentScorePoco = new IdeaAssessmentScorePoco()
            {
                IdeaId = ideaId,
                Score = 50
            };
            var ideaAssessmentScoreResult = await _ideaAssessmentScoresController.PostIdeaAssessmentScore(ideaAssessmentScorePoco);
            Assert.NotNull(ideaAssessmentScoreResult);
            Assert.NotNull(ideaAssessmentScoreResult.Result);
            var ideaAssessmentScoreResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentScoreResult.Result);
            Assert.NotEqual(ideaAssessmentScoreResultObject.Value, Guid.Empty);
            var ideaAssessmentScoreId = Assert.IsType<Guid>(ideaAssessmentScoreResultObject.Value);

            // view
            var ideaViewResult = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult);
            Assert.NotNull(ideaViewResult.Result);
            var ideaViewResultObject = Assert.IsType<OkObjectResult>(ideaViewResult.Result);
            var ideaViewPocoObject = Assert.IsType<IdeaViewPoco>(ideaViewResultObject.Value);
            Assert.Equal(ideaViewPocoObject.Id, ideaId);
            Assert.Single(ideaViewPocoObject.IdeaAssessmentScores);


            // view
            var ideaAssessmentScoreViewResult = await _ideaAssessmentScoresController.GetIdeaAssessmentScore(ideaAssessmentScoreId);
            Assert.NotNull(ideaAssessmentScoreViewResult);
            Assert.NotNull(ideaAssessmentScoreViewResult.Result);
            var ideaAssessmentScoreViewResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentScoreViewResult.Result);
            var ideaAssessmentScoreViewPocoObject = Assert.IsType<IdeaAssessmentScoreViewPoco>(ideaAssessmentScoreViewResultObject.Value);
            Assert.Equal(ideaAssessmentScoreViewPocoObject.Id, ideaAssessmentScoreId);
            Assert.NotNull(ideaAssessmentScoreViewPocoObject.IdeaAssessmentScore);

            // update
            var newValue = 80;
            ideaAssessmentScorePoco.Score = newValue;
            var ideaAssessmentScoreUpdateResult = await _ideaAssessmentScoresController.PutIdeaAssessmentScore(ideaAssessmentScoreId, ideaAssessmentScorePoco);
            Assert.NotNull(ideaAssessmentScoreUpdateResult);
            Assert.NotNull(ideaAssessmentScoreUpdateResult.Result);
            var ideaAssessmentScoreUpdateResultObject = Assert.IsType<OkObjectResult>(ideaAssessmentScoreUpdateResult.Result);
            Assert.Equal(ideaAssessmentScoreUpdateResultObject.Value, ideaAssessmentScoreId);

            // re check
            var ideaAssessmentScoreViewResult2 = await _ideaAssessmentScoresController.GetIdeaAssessmentScore(ideaAssessmentScoreId);
            Assert.NotNull(ideaAssessmentScoreViewResult2);
            Assert.NotNull(ideaAssessmentScoreViewResult2.Result);
            var ideaAssessmentScoreViewResultObject2 = Assert.IsType<OkObjectResult>(ideaAssessmentScoreViewResult2.Result);
            var ideaAssessmentScoreViewPocoObject2 = Assert.IsType<IdeaAssessmentScoreViewPoco>(ideaAssessmentScoreViewResultObject2.Value);
            Assert.Equal(ideaAssessmentScoreViewPocoObject2.Id, ideaAssessmentScoreId);
            Assert.Equal(ideaAssessmentScoreViewPocoObject2.IdeaAssessmentScore.Score, newValue);

            // delete
            var deleteResult = await _ideaAssessmentScoresController.DeleteIdeaAssessmentScore(ideaAssessmentScoreId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaAssessmentScoreId);


            // view 
            var ideaAssessmentScoreViewResult3 = await _ideaAssessmentScoresController.GetIdeaAssessmentScore(ideaAssessmentScoreId);
            Assert.NotNull(ideaAssessmentScoreViewResult3);
            Assert.NotNull(ideaAssessmentScoreViewResult3.Result);
            var ideaAssessmentScoreViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaAssessmentScoreViewResult3.Result);
            Assert.Equal(ideaAssessmentScoreViewPocoObject3.Value, ideaAssessmentScoreId);

        }
    }
}
