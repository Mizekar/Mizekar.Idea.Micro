using System;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Services;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsServicesController
    {
        private readonly ServicesController _servicesController;

        public UnitTestsServicesController()
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
            _servicesController = new ServicesController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudService()
        {
            
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


            // view
            var serviceViewResult = await _servicesController.GetServiceInfo(serviceId);
            Assert.NotNull(serviceViewResult);
            Assert.NotNull(serviceViewResult.Result);
            var serviceViewResultObject = Assert.IsType<OkObjectResult>(serviceViewResult.Result);
            var serviceViewPocoObject = Assert.IsType<ServiceViewPoco>(serviceViewResultObject.Value);
            Assert.Equal(serviceViewPocoObject.Id, serviceId);
            Assert.NotNull(serviceViewPocoObject.Service);

            // update
            var serviceTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            servicePoco.Title = serviceTitle;
            var serviceUpdateResult = await _servicesController.PutServiceInfo(serviceId, servicePoco);
            Assert.NotNull(serviceUpdateResult);
            Assert.NotNull(serviceUpdateResult.Result);
            var serviceUpdateResultObject = Assert.IsType<OkObjectResult>(serviceUpdateResult.Result);
            Assert.Equal(serviceUpdateResultObject.Value, serviceId);

            // re check
            var serviceViewResult2 = await _servicesController.GetServiceInfo(serviceId);
            Assert.NotNull(serviceViewResult2);
            Assert.NotNull(serviceViewResult2.Result);
            var serviceViewResultObject2 = Assert.IsType<OkObjectResult>(serviceViewResult2.Result);
            var serviceViewPocoObject2 = Assert.IsType<ServiceViewPoco>(serviceViewResultObject2.Value);
            Assert.Equal(serviceViewPocoObject2.Id, serviceId);
            Assert.Equal(serviceViewPocoObject2.Service.Title, serviceTitle);

            // delete
            var deleteResult = await _servicesController.DeleteService(serviceId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, serviceId);


            // view 
            var serviceViewResult3 = await _servicesController.GetServiceInfo(serviceId);
            Assert.NotNull(serviceViewResult3);
            Assert.NotNull(serviceViewResult3.Result);
            var serviceViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(serviceViewResult3.Result);
            Assert.Equal(serviceViewPocoObject3.Value, serviceId);

        }
    }
}
