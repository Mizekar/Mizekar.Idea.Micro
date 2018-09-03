using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Model.Api.Response;
using Mizekar.Micro.Idea.Controllers;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.MapProfiles;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Permissions;
using Xunit;

namespace Mizekar.Micro.Idea.Tests
{
    public class UnitTestsPermissionsController
    {
        private readonly PermissionsController _permissionsController;
        private readonly IdeasController _ideasController;
        private readonly IdeaStatusesController _ideaStatusesController;

        public UnitTestsPermissionsController()
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
            _permissionsController = new PermissionsController(context, imapper);
        }

        private DbContextOptions<IdeaDbContext> DbOptionsSqlite { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseSqlite(string.Format("Data Source={0}.db", Guid.NewGuid().ToString("N")))
            .Options;

        private DbContextOptions<IdeaDbContext> DbOptionsInMemory { get; } = new DbContextOptionsBuilder<IdeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public async void CrudPermission()
        {
            var userId = 1;


            var permissionPoco = new PermissionPoco()
            {
                Order = 1,
                Title = "test title",
                Name = "test name",
            };
            var permissionResult = await _permissionsController.PostPermission(permissionPoco);
            Assert.NotNull(permissionResult);
            Assert.NotNull(permissionResult.Result);
            var permissionResultObject = Assert.IsType<OkObjectResult>(permissionResult.Result);
            Assert.NotEqual(permissionResultObject.Value, Guid.Empty);
            var permissionId = Assert.IsType<Guid>(permissionResultObject.Value);

            var permissionPoco2 = new PermissionPoco()
            {
                Order = 1,
                Title = "test title",
                Name = "test name",
            };
            var permissionResult2 = await _permissionsController.PostPermission(permissionPoco2);
            Assert.NotNull(permissionResult2);
            Assert.NotNull(permissionResult2.Result);
            var permissionResultObject2 = Assert.IsType<OkObjectResult>(permissionResult2.Result);
            Assert.NotEqual(permissionResultObject2.Value, Guid.Empty);
            var permissionId2 = Assert.IsType<Guid>(permissionResultObject2.Value);

            // view All
            var permissionViewAllResult = await _permissionsController.GetPermissions(1, 100);
            Assert.NotNull(permissionViewAllResult);
            Assert.NotNull(permissionViewAllResult.Result);
            var permissionViewAllResultObject = Assert.IsType<OkObjectResult>(permissionViewAllResult.Result);
            var permissionViewAllPocoObject = Assert.IsType<Paged<PermissionViewPoco>>(permissionViewAllResultObject.Value);
            Assert.NotNull(permissionViewAllPocoObject);
            Assert.NotNull(permissionViewAllPocoObject.Items);
            Assert.Equal(2, permissionViewAllPocoObject.TotalCount);

            var resultSet1 = await _permissionsController.SetPermissionsForUserId(userId, new Guid[] { permissionId });
            var permissionsForUser1 = await _permissionsController.GetPermissionsByUserId(userId);
            Assert.NotNull(permissionsForUser1);
            Assert.NotNull(permissionsForUser1.Result);
            var permissionsForUserResultObject1 = Assert.IsType<OkObjectResult>(permissionsForUser1.Result);
            var permissionsForUserResultObjects1 = Assert.IsType<List<PermissionViewPoco>>(permissionsForUserResultObject1.Value);
            Assert.NotNull(permissionsForUserResultObjects1);
            Assert.Single(permissionsForUserResultObjects1);
            Assert.Equal(permissionsForUserResultObjects1.First().Id, permissionId);

            var resultSet2 = await _permissionsController.SetPermissionsForUserId(userId, new Guid[] { permissionId2 });
            var permissionsForUser2 = await _permissionsController.GetPermissionsByUserId(userId);
            Assert.NotNull(permissionsForUser2);
            Assert.NotNull(permissionsForUser2.Result);
            var permissionsForUserResultObject2 = Assert.IsType<OkObjectResult>(permissionsForUser2.Result);
            var permissionsForUserResultObjects2 = Assert.IsType<List<PermissionViewPoco>>(permissionsForUserResultObject2.Value);
            Assert.NotNull(permissionsForUserResultObjects2);
            Assert.Single(permissionsForUserResultObjects2);
            Assert.Equal(permissionsForUserResultObjects2.First().Id, permissionId2);

            var resultSet3 = await _permissionsController.SetPermissionsForUserId(userId, new Guid[] { permissionId, permissionId2 });
            var permissionsForUser3 = await _permissionsController.GetPermissionsByUserId(userId);
            Assert.NotNull(permissionsForUser3);
            Assert.NotNull(permissionsForUser3.Result);
            var permissionsForUserResultObject3 = Assert.IsType<OkObjectResult>(permissionsForUser3.Result);
            var permissionsForUserResultObjects3 = Assert.IsType<List<PermissionViewPoco>>(permissionsForUserResultObject3.Value);
            Assert.NotNull(permissionsForUserResultObjects3);
            Assert.Equal(2, permissionsForUserResultObjects3.Count);


            // view
            var permissionViewResult = await _permissionsController.GetPermissionInfo(permissionId);
            Assert.NotNull(permissionViewResult);
            Assert.NotNull(permissionViewResult.Result);
            var permissionViewResultObject = Assert.IsType<OkObjectResult>(permissionViewResult.Result);
            var permissionViewPocoObject = Assert.IsType<PermissionViewPoco>(permissionViewResultObject.Value);
            Assert.Equal(permissionViewPocoObject.Id, permissionId);
            Assert.NotNull(permissionViewPocoObject.Permission);

            // update
            var permissionTitle = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            permissionPoco.Title = permissionTitle;
            var permissionUpdateResult = await _permissionsController.PutPermissionInfo(permissionId, permissionPoco);
            Assert.NotNull(permissionUpdateResult);
            Assert.NotNull(permissionUpdateResult.Result);
            var permissionUpdateResultObject = Assert.IsType<OkObjectResult>(permissionUpdateResult.Result);
            Assert.Equal(permissionUpdateResultObject.Value, permissionId);

            // re check
            var permissionViewResult2 = await _permissionsController.GetPermissionInfo(permissionId);
            Assert.NotNull(permissionViewResult2);
            Assert.NotNull(permissionViewResult2.Result);
            var permissionViewResultObject2 = Assert.IsType<OkObjectResult>(permissionViewResult2.Result);
            var permissionViewPocoObject2 = Assert.IsType<PermissionViewPoco>(permissionViewResultObject2.Value);
            Assert.Equal(permissionViewPocoObject2.Id, permissionId);
            Assert.Equal(permissionViewPocoObject2.Permission.Title, permissionTitle);

            // delete
            var deleteResult = await _permissionsController.DeletePermission(permissionId);
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Result);
            var deleteResultObject = Assert.IsType<OkObjectResult>(deleteResult.Result);
            Assert.Equal(deleteResultObject.Value, permissionId);

            // delete 2
            var deleteResult2 = await _permissionsController.DeletePermission(permissionId2);
            Assert.NotNull(deleteResult2);
            Assert.NotNull(deleteResult2.Result);
            var deleteResultObject2 = Assert.IsType<OkObjectResult>(deleteResult2.Result);
            Assert.Equal(deleteResultObject2.Value, permissionId2);

            // view 
            var permissionViewResult3 = await _permissionsController.GetPermissionInfo(permissionId);
            Assert.NotNull(permissionViewResult3);
            Assert.NotNull(permissionViewResult3.Result);
            var permissionViewPocoObject3 = Assert.IsType<NotFoundObjectResult>(permissionViewResult3.Result);
            Assert.Equal(permissionViewPocoObject3.Value, permissionId);

        }
    }
}
