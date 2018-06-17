//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.Configuration;
//using Xunit;

//namespace Mizekar.Micro.Idea.Tests
//{
//    public class UnitTestApi_Idea
//    {
//        private readonly TestServer _server;
//        private readonly HttpClient _client;
//        public UnitTestApi_Idea()
//        {
//            var configs = new List<KeyValuePair<string, string>>();
//            configs.Add(new KeyValuePair<string, string>("dbconnection", "Server=.;Database=Mizekar.Accounts;User Id=sa;Password=123456;MultipleActiveResultSets=True"));

//            // Arrange
//            _server = new TestServer(new WebHostBuilder()
//                .UseEnvironment("Development")
//                .UseConfiguration(new ConfigurationBuilder()
//                    //.AddInMemoryCollection(configs)
//                    .Build())
//                .UseStartup<Startup>());
//            _client = _server.CreateClient();
//        }

//        [Fact]
//        public void ProfileApi_GetAll()
//        {
//            var profilesMicroService = new ProfilesMicroService(_client);
//            var result = profilesMicroService.GetAsync().Result;
//            Assert.NotNull(result.Profiles);
//            Assert.NotNull(result.ResponseStatus);
//            Assert.True(result.ResponseStatus.IsSuccess);
//        }

//        [Fact]
//        public void ProfileApi_NewProfile()
//        {
//            var profilesMicroService = new ProfilesMicroService(_client);

//            // new
//            var newProfile = new ProfileNew()
//            {
//                FirstName = "reza",
//            };
//            var result = profilesMicroService.PostAsync(newProfile).Result;
//            Assert.NotNull(result);
//            Assert.True(result.IsSuccess);
//            int profileId = int.Parse(result.Message);
//            Assert.True(profileId > 0);

//            // get
//            var getResult = profilesMicroService.GetByIdAsync(profileId).Result;
//            Assert.NotNull(getResult);
//            Assert.True(getResult.ResponseStatus.IsSuccess);
//            Assert.Equal(getResult.Profile.Id, profileId);

//            // update
//            var updateProfile = new ProfileUpdate()
//            {
//                FirstName = "reza-update",
//            };
//            var updateResult = profilesMicroService.PutAsync(profileId, updateProfile).Result;
//            Assert.NotNull(updateResult);
//            Assert.True(updateResult.IsSuccess);

//        }
//    }
//}
