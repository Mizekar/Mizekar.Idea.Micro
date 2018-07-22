using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Announcements;
using Mizekar.Micro.Idea.Models.Services;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsIdeasController
    {
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;
        private readonly AnnouncementsController _announcementsController;
        private readonly ServicesController _servicesController;


        public UnitTestsIdeasController()
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
            _servicesController = new ServicesController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudIdea()
        {
            var status = new IdeaStatusPoco() { Order = 1, Title = "انتشار اولیه" };
            var statusResult = await _ideaStatusesController.PostIdeaStatus(status);

            Assert.NotNull(statusResult);
            Assert.NotNull(statusResult.Result);
            var statusResultObject = Assert.IsType<OkObjectResult>(statusResult.Result);
            Assert.NotEqual(statusResultObject.Value, Guid.Empty);
            var statusId = Assert.IsType<Guid>(statusResultObject.Value);

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

            var servicePoco = new ServicePoco()
            {
                Order = 1,
                Title = "عنوان",
                Description = "توضیحات",
            };
            var serviceResult = await _servicesController.PostService(servicePoco);
            Assert.NotNull(serviceResult);
            Assert.NotNull(serviceResult.Result);
            var serviceResultObject = Assert.IsType<OkObjectResult>(serviceResult.Result);
            Assert.NotEqual(serviceResultObject.Value, Guid.Empty);
            var serviceId = Assert.IsType<Guid>(serviceResultObject.Value);

            var userId = 1;
            var ideaPoco = new Models.IdeaPoco()
            {
                Text = "ایده من",
                Title = "عنوان ایده",
                IsDraft = false,
                OwnerId = userId,
                IdeaStatusId = statusId,
                AnnouncementId = announcementId,
                ServiceId = serviceId,
                PriorityByOwner = 5
            };
            var ideaResult = await _ideasController.PostIdea(ideaPoco);
            Assert.NotNull(ideaResult);
            Assert.NotNull(ideaResult.Result);
            var ideaResultObject = Assert.IsType<OkObjectResult>(ideaResult.Result);
            Assert.NotEqual(ideaResultObject.Value, Guid.Empty);
            var ideaId = Assert.IsType<Guid>(ideaResultObject.Value);

            // view
            var ideaViewResult = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult);
            Assert.NotNull(ideaViewResult.Result);
            var ideaViewResultObject = Assert.IsType<OkObjectResult>(ideaViewResult.Result);
            var ideaViewPocoObject = Assert.IsType<IdeaViewPoco>(ideaViewResultObject.Value);
            Assert.Equal(ideaViewPocoObject.Id, ideaId);
            Assert.NotNull(ideaViewPocoObject.Idea);
            Assert.NotNull(ideaViewPocoObject.IdeaStatus);
            Assert.NotNull(ideaViewPocoObject.Announcement);
            Assert.NotNull(ideaViewPocoObject.Service);
            Assert.NotNull(ideaViewPocoObject.AdvancedField);
            Assert.NotNull(ideaViewPocoObject.BusinessBaseInfo);
            Assert.NotNull(ideaViewPocoObject.SocialStatistic);

            Assert.Equal(ideaViewPocoObject.Announcement.Title, announcementPoco.Title);
            Assert.Equal(ideaViewPocoObject.Service.Title, servicePoco.Title);

            // update
            var ideaText = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            ideaPoco.Text = ideaText;
            var ideaUpdateResult = await _ideasController.PutIdeaInfo(ideaId, ideaPoco);
            Assert.NotNull(ideaUpdateResult);
            Assert.NotNull(ideaUpdateResult.Result);
            var ideaUpdateResultObject = Assert.IsType<OkObjectResult>(ideaUpdateResult.Result);
            Assert.Equal(ideaUpdateResultObject.Value, ideaId);

            // re check
            var ideaViewResult2 = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult2);
            Assert.NotNull(ideaViewResult2.Result);
            var ideaViewResultObject2 = Assert.IsType<OkObjectResult>(ideaViewResult2.Result);
            var ideaViewPocoObject2 = Assert.IsType<IdeaViewPoco>(ideaViewResultObject2.Value);
            Assert.Equal(ideaViewPocoObject2.Id, ideaId);
            Assert.Equal(ideaViewPocoObject2.Idea.Text, ideaText);

            // delete
            var deleteResult = await _ideasController.DeleteIdea(ideaId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, ideaId);


            // view 
            var ideaViewResult3 = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult3);
            Assert.NotNull(ideaViewResult3.Result);
            var ideaViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(ideaViewResult3.Result);
            Assert.Equal(ideaViewPocoObject3.Value, ideaId);

        }

        [Fact]
        public async void CrudIdeaAdvanced()
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

            // create
            var departmentLinks = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var scopeLinks = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var strategyLinks = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var subjectLinks = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var advanced = new IdeaAdvancedFieldPoco()
            {
                Achievement = "دست آورد ها",
                Details = "شرح کامل ایده",
                DepartmentLinks = departmentLinks,
                ScopeLinks = scopeLinks,
                StrategyLinks = strategyLinks,
                SubjectLinks = subjectLinks
            };
            var ideaAdvancedResult = await _ideasController.PutIdeaAdvancedFields(ideaId, advanced);
            Assert.NotNull(ideaAdvancedResult);
            Assert.NotNull(ideaAdvancedResult.Result);
            var ideaAdvancedResultObject = Assert.IsType<OkObjectResult>(ideaAdvancedResult.Result);
            Assert.Equal(ideaAdvancedResultObject.Value, ideaId);

            // view 
            var ideaViewResult = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult);
            Assert.NotNull(ideaViewResult.Result);
            var ideaViewResultObject = Assert.IsType<OkObjectResult>(ideaViewResult.Result);
            var ideaViewPocoObject = Assert.IsType<IdeaViewPoco>(ideaViewResultObject.Value);
            Assert.Equal(ideaViewPocoObject.Id, ideaId);
            Assert.NotNull(ideaViewPocoObject.Idea);
            Assert.NotNull(ideaViewPocoObject.AdvancedField);
            Assert.NotNull(ideaViewPocoObject.BusinessBaseInfo);
            Assert.NotNull(ideaViewPocoObject.SocialStatistic);
            Assert.Equal(ideaViewPocoObject.AdvancedField.StrategyLinks, strategyLinks);
            Assert.Equal(ideaViewPocoObject.AdvancedField.ScopeLinks, scopeLinks);
            Assert.Equal(ideaViewPocoObject.AdvancedField.SubjectLinks, subjectLinks);
            Assert.Equal(ideaViewPocoObject.AdvancedField.DepartmentLinks, departmentLinks);


            // update advanced 
            departmentLinks.Remove(departmentLinks.Last());
            departmentLinks.Add(Guid.NewGuid());
            departmentLinks.Add(Guid.NewGuid());
            var advancedUpdate = new IdeaAdvancedFieldPoco()
            {
                Achievement = "دست آورد ها",
                Details = "شرح کامل ایده",
                DepartmentLinks = departmentLinks,
                ScopeLinks = scopeLinks,
                StrategyLinks = strategyLinks,
                SubjectLinks = subjectLinks
            };
            var ideaAdvancedUpdateResult = await _ideasController.PutIdeaAdvancedFields(ideaId, advancedUpdate);
            Assert.NotNull(ideaAdvancedUpdateResult);
            Assert.NotNull(ideaAdvancedUpdateResult.Result);
            var ideaAdvancedUpdateResultObject = Assert.IsType<OkObjectResult>(ideaAdvancedUpdateResult.Result);
            Assert.Equal(ideaAdvancedUpdateResultObject.Value, ideaId);

            // view 
            var ideaViewResult2 = await _ideasController.GetIdeaInfo(ideaId);
            Assert.NotNull(ideaViewResult2);
            Assert.NotNull(ideaViewResult2.Result);
            var ideaViewResultObject2 = Assert.IsType<OkObjectResult>(ideaViewResult2.Result);
            var ideaViewPocoObject2 = Assert.IsType<IdeaViewPoco>(ideaViewResultObject2.Value);
            Assert.Equal(ideaViewPocoObject2.AdvancedField.DepartmentLinks, departmentLinks);

        }
        
    }
}
