using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using NJsonSchema;
using NSwag.AspNetCore;
// ReSharper disable UnusedMember.Global

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
                _logger.LogWarning("Database Use InMemory Database");
            }
            else
            {
                services.AddDbContext<IdeaDbContext>(options => options.UseSqlServer(accountsDbConnection));
                _logger.LogInformation("Database Use Sql Server");
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // services.AddSwagger(); // only needed for the UseSwaggger*WithApiExplorer() methods (below)
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
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            var assembly = Assembly.GetEntryAssembly();
            var productTitle = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            var productDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            var productVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            app.UseSwaggerReDoc(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultEnumHandling = EnumHandling.String;
                settings.GeneratorSettings.Title = productTitle;
                settings.GeneratorSettings.Description = productDescription;
                settings.GeneratorSettings.Version = productVersion;
            });

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
