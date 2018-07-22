using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Announcements;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsAnnouncementsController
    {
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;
        private readonly AnnouncementsController _announcementsController;

        public UnitTestsAnnouncementsController()
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
            _announcementsController = new AnnouncementsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudAnnouncement()
        {
            var announcementPoco = new AnnouncementPoco()
            {
                Order = 1,
                Title = "عنوان",
                Description = "توضیحات",
            };
            var announcementResult = await _announcementsController.PostAnnouncement(announcementPoco);
            Assert.NotNull(announcementResult);
            Assert.NotNull(announcementResult.Result);
            var announcementResultObject = Assert.IsType<OkObjectResult>(announcementResult.Result);
            Assert.NotEqual(announcementResultObject.Value, Guid.Empty);
            var announcementId = Assert.IsType<Guid>(announcementResultObject.Value);

            var statusId = await CreateIdeaStatus();
            var ideaId = await CreateIdea(statusId, announcementId);

            // view
            var announcementViewResult = await _announcementsController.GetAnnouncementInfo(announcementId);
            Assert.NotNull(announcementViewResult);
            Assert.NotNull(announcementViewResult.Result);
            var announcementViewResultObject = Assert.IsType<OkObjectResult>(announcementViewResult.Result);
            var announcementViewPocoObject = Assert.IsType<AnnouncementViewPoco>(announcementViewResultObject.Value);
            Assert.Equal(announcementViewPocoObject.Id, announcementId);
            Assert.Equal(1, announcementViewPocoObject.RelatedIdeasCount);
            Assert.NotNull(announcementViewPocoObject.Announcement);

            // update
            var announcementTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            announcementPoco.Title = announcementTitle;
            var announcementUpdateResult = await _announcementsController.PutAnnouncementInfo(announcementId, announcementPoco);
            Assert.NotNull(announcementUpdateResult);
            Assert.NotNull(announcementUpdateResult.Result);
            var announcementUpdateResultObject = Assert.IsType<OkObjectResult>(announcementUpdateResult.Result);
            Assert.Equal(announcementUpdateResultObject.Value, announcementId);

            // re check
            var announcementViewResult2 = await _announcementsController.GetAnnouncementInfo(announcementId);
            Assert.NotNull(announcementViewResult2);
            Assert.NotNull(announcementViewResult2.Result);
            var announcementViewResultObject2 = Assert.IsType<OkObjectResult>(announcementViewResult2.Result);
            var announcementViewPocoObject2 = Assert.IsType<AnnouncementViewPoco>(announcementViewResultObject2.Value);
            Assert.Equal(announcementViewPocoObject2.Id, announcementId);
            Assert.Equal(announcementViewPocoObject2.Announcement.Title, announcementTitle);

            // delete
            var deleteResult = await _announcementsController.DeleteAnnouncement(announcementId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, announcementId);


            // view 
            var announcementViewResult3 = await _announcementsController.GetAnnouncementInfo(announcementId);
            Assert.NotNull(announcementViewResult3);
            Assert.NotNull(announcementViewResult3.Result);
            var announcementViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(announcementViewResult3.Result);
            Assert.Equal(announcementViewPocoObject3.Value, announcementId);

        }

        private async Task<Guid> CreateIdea(Guid statusId, Guid announcementId)
        {
            var userId = 1;
            var ideaPoco = new Models.IdeaPoco()
            {
                Text = "ایده من",
                Title = "عنوان ایده",
                IsDraft = false,
                OwnerId = userId,
                IdeaStatusId = statusId,
                AnnouncementId = announcementId,
                PriorityByOwner = 5
            };
            var ideaResult = await _ideasController.PostIdea(ideaPoco);
            Assert.NotNull(ideaResult);
            Assert.NotNull(ideaResult.Result);
            var ideaResultObject = Assert.IsType<OkObjectResult>(ideaResult.Result);
            Assert.NotEqual(ideaResultObject.Value, Guid.Empty);
            var ideaId = Assert.IsType<Guid>(ideaResultObject.Value);
            return ideaId;
        }

        private async Task<Guid> CreateIdeaStatus()
        {
            var status = new IdeaStatusPoco() { Order = 1, Title = "انتشار اولیه" };
            var statusResult = await _ideaStatusesController.PostIdeaStatus(status);

            Assert.NotNull(statusResult);
            Assert.NotNull(statusResult.Result);
            var statusResultObject = Assert.IsType<OkObjectResult>(statusResult.Result);
            Assert.NotEqual(statusResultObject.Value, Guid.Empty);
            var statusId = Assert.IsType<Guid>(statusResultObject.Value);
            return statusId;
        }
    }
}
