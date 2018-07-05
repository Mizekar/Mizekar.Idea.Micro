using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Participations;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsParticipationsController
    {
        private readonly ParticipationsController _participationsController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsParticipationsController()
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
            _participationsController = new ParticipationsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudParticipation()
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


            var participationPoco = new ParticipationPoco()
            {
                IdeaId = ideaId,
                FullName = "نام",
                Description = "توضیحات",
            };
            var participationResult = await _participationsController.PostParticipation(participationPoco);
            Assert.NotNull(participationResult);
            Assert.NotNull(participationResult.Result);
            var participationResultObject = Assert.IsType<OkObjectResult>(participationResult.Result);
            Assert.NotEqual(participationResultObject.Value, Guid.Empty);
            var participationId = Assert.IsType<Guid>(participationResultObject.Value);


            // view
            var participationViewResult = await _participationsController.GetParticipationInfo(participationId);
            Assert.NotNull(participationViewResult);
            Assert.NotNull(participationViewResult.Result);
            var participationViewResultObject = Assert.IsType<OkObjectResult>(participationViewResult.Result);
            var participationViewPocoObject = Assert.IsType<ParticipationViewPoco>(participationViewResultObject.Value);
            Assert.Equal(participationViewPocoObject.Id, participationId);
            Assert.NotNull(participationViewPocoObject.Participation);

            // update
            var participationTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            participationPoco.Description = participationTitle;
            var participationUpdateResult = await _participationsController.PutParticipationInfo(participationId, participationPoco);
            Assert.NotNull(participationUpdateResult);
            Assert.NotNull(participationUpdateResult.Result);
            var participationUpdateResultObject = Assert.IsType<OkObjectResult>(participationUpdateResult.Result);
            Assert.Equal(participationUpdateResultObject.Value, participationId);

            // re check
            var participationViewResult2 = await _participationsController.GetParticipationInfo(participationId);
            Assert.NotNull(participationViewResult2);
            Assert.NotNull(participationViewResult2.Result);
            var participationViewResultObject2 = Assert.IsType<OkObjectResult>(participationViewResult2.Result);
            var participationViewPocoObject2 = Assert.IsType<ParticipationViewPoco>(participationViewResultObject2.Value);
            Assert.Equal(participationViewPocoObject2.Id, participationId);
            Assert.Equal(participationViewPocoObject2.Participation.Description, participationTitle);

            // delete
            var deleteResult = await _participationsController.DeleteParticipation(participationId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, participationId);


            // view 
            var participationViewResult3 = await _participationsController.GetParticipationInfo(participationId);
            Assert.NotNull(participationViewResult3);
            Assert.NotNull(participationViewResult3.Result);
            var participationViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(participationViewResult3.Result);
            Assert.Equal(participationViewPocoObject3.Value, participationId);

        }
    }
}
