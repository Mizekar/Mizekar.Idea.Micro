using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mizekar.Idea.Micro.Data;

namespace Mizekar.Idea.Micro
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this._logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var accountsDbConnection = Configuration["dbconnection"]; // docker config
            if (accountsDbConnection == null)
            {
                accountsDbConnection = Configuration.GetConnectionString("dbconnection"); // app config
            }

            if (string.IsNullOrEmpty(accountsDbConnection))
            {
                services.AddDbContext<IdeaDbContext>(op => { op.UseInMemoryDatabase(Guid.NewGuid().ToString()); });
                _logger.LogWarning("Database Use InMemory type");
            }
            else
            {
                services.AddDbContext<IdeaDbContext>(options => options.UseSqlServer(accountsDbConnection));
                _logger.LogInformation("Database Use sql server");
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IdeaDbContext>();
                if (!context.Database.IsInMemory())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
