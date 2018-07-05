using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Operational;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsOperationalPhasesController
    {
        private readonly OperationalPhasesController _operationalPhasesController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsOperationalPhasesController()
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
            _operationalPhasesController = new OperationalPhasesController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudOperationalPhase()
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


            var operationalPhasePoco = new OperationalPhasePoco()
            {
                IdeaId = ideaId,
                Order = 1,
                Title = "عنوان فاز",
                Description = "توضیحات",
                MoneyRequired = 100000000000,
                TimeRequiredByDays = 365
            };
            var operationalPhaseResult = await _operationalPhasesController.PostOperationalPhase(operationalPhasePoco);
            Assert.NotNull(operationalPhaseResult);
            Assert.NotNull(operationalPhaseResult.Result);
            var operationalPhaseResultObject = Assert.IsType<OkObjectResult>(operationalPhaseResult.Result);
            Assert.NotEqual(operationalPhaseResultObject.Value, Guid.Empty);
            var operationalPhaseId = Assert.IsType<Guid>(operationalPhaseResultObject.Value);


            // view
            var operationalPhaseViewResult = await _operationalPhasesController.GetOperationalPhaseInfo(operationalPhaseId);
            Assert.NotNull(operationalPhaseViewResult);
            Assert.NotNull(operationalPhaseViewResult.Result);
            var operationalPhaseViewResultObject = Assert.IsType<OkObjectResult>(operationalPhaseViewResult.Result);
            var operationalPhaseViewPocoObject = Assert.IsType<OperationalPhaseViewPoco>(operationalPhaseViewResultObject.Value);
            Assert.Equal(operationalPhaseViewPocoObject.Id, operationalPhaseId);
            Assert.NotNull(operationalPhaseViewPocoObject.OperationalPhase);

            // update
            var operationalPhaseTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            operationalPhasePoco.Title = operationalPhaseTitle;
            var operationalPhaseUpdateResult = await _operationalPhasesController.PutOperationalPhaseInfo(operationalPhaseId, operationalPhasePoco);
            Assert.NotNull(operationalPhaseUpdateResult);
            Assert.NotNull(operationalPhaseUpdateResult.Result);
            var operationalPhaseUpdateResultObject = Assert.IsType<OkObjectResult>(operationalPhaseUpdateResult.Result);
            Assert.Equal(operationalPhaseUpdateResultObject.Value, operationalPhaseId);

            // re check
            var operationalPhaseViewResult2 = await _operationalPhasesController.GetOperationalPhaseInfo(operationalPhaseId);
            Assert.NotNull(operationalPhaseViewResult2);
            Assert.NotNull(operationalPhaseViewResult2.Result);
            var operationalPhaseViewResultObject2 = Assert.IsType<OkObjectResult>(operationalPhaseViewResult2.Result);
            var operationalPhaseViewPocoObject2 = Assert.IsType<OperationalPhaseViewPoco>(operationalPhaseViewResultObject2.Value);
            Assert.Equal(operationalPhaseViewPocoObject2.Id, operationalPhaseId);
            Assert.Equal(operationalPhaseViewPocoObject2.OperationalPhase.Title, operationalPhaseTitle);

            // delete
            var deleteResult = await _operationalPhasesController.DeleteOperationalPhase(operationalPhaseId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, operationalPhaseId);


            // view 
            var operationalPhaseViewResult3 = await _operationalPhasesController.GetOperationalPhaseInfo(operationalPhaseId);
            Assert.NotNull(operationalPhaseViewResult3);
            Assert.NotNull(operationalPhaseViewResult3.Result);
            var operationalPhaseViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(operationalPhaseViewResult3.Result);
            Assert.Equal(operationalPhaseViewPocoObject3.Value, operationalPhaseId);

        }
        
    }
}
