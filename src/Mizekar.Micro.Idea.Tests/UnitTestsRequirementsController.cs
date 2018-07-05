using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Requirements;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsRequirementsController
    {
        private readonly RequirementsController _requirementsController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsRequirementsController()
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
            _requirementsController = new RequirementsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudRequirement()
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


            var requirementPoco = new RequirementPoco()
            {
                IdeaId = ideaId,
                Order = 1,
                Title = "عنوان فاز",
                Description = "توضیحات",
                MoneyRequired = 100000000000,
                TimeRequiredByDays = 365
            };
            var requirementResult = await _requirementsController.PostRequirement(requirementPoco);
            Assert.NotNull(requirementResult);
            Assert.NotNull(requirementResult.Result);
            var requirementResultObject = Assert.IsType<OkObjectResult>(requirementResult.Result);
            Assert.NotEqual(requirementResultObject.Value, Guid.Empty);
            var requirementId = Assert.IsType<Guid>(requirementResultObject.Value);


            // view
            var requirementViewResult = await _requirementsController.GetRequirementInfo(requirementId);
            Assert.NotNull(requirementViewResult);
            Assert.NotNull(requirementViewResult.Result);
            var requirementViewResultObject = Assert.IsType<OkObjectResult>(requirementViewResult.Result);
            var requirementViewPocoObject = Assert.IsType<RequirementViewPoco>(requirementViewResultObject.Value);
            Assert.Equal(requirementViewPocoObject.Id, requirementId);
            Assert.NotNull(requirementViewPocoObject.Requirement);

            // update
            var requirementTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            requirementPoco.Title = requirementTitle;
            var requirementUpdateResult = await _requirementsController.PutRequirementInfo(requirementId, requirementPoco);
            Assert.NotNull(requirementUpdateResult);
            Assert.NotNull(requirementUpdateResult.Result);
            var requirementUpdateResultObject = Assert.IsType<OkObjectResult>(requirementUpdateResult.Result);
            Assert.Equal(requirementUpdateResultObject.Value, requirementId);

            // re check
            var requirementViewResult2 = await _requirementsController.GetRequirementInfo(requirementId);
            Assert.NotNull(requirementViewResult2);
            Assert.NotNull(requirementViewResult2.Result);
            var requirementViewResultObject2 = Assert.IsType<OkObjectResult>(requirementViewResult2.Result);
            var requirementViewPocoObject2 = Assert.IsType<RequirementViewPoco>(requirementViewResultObject2.Value);
            Assert.Equal(requirementViewPocoObject2.Id, requirementId);
            Assert.Equal(requirementViewPocoObject2.Requirement.Title, requirementTitle);

            // delete
            var deleteResult = await _requirementsController.DeleteRequirement(requirementId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, requirementId);


            // view 
            var requirementViewResult3 = await _requirementsController.GetRequirementInfo(requirementId);
            Assert.NotNull(requirementViewResult3);
            Assert.NotNull(requirementViewResult3.Result);
            var requirementViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(requirementViewResult3.Result);
            Assert.Equal(requirementViewPocoObject3.Value, requirementId);

        }
    }
}
