using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Mizekar.Micro.Idea.Data;

namespace Mizekar.Micro.Idea
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var host = BuildWebHost(args);
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var serviceProvider = services.GetRequiredService<IServiceProvider>();
                        var configuration = services.GetRequiredService<IConfiguration>();
                        var context = services.GetRequiredService<IdeaDbContext>();
                        DataBaseManager.Migrate(context);
                        //DataBaseManager.CreatePermissions(context).Wait();

                    }
                    catch (Exception exception)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(exception, "An error occurred while creating roles");
                    }
                }

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    if (hostingContext.Configuration.GetValue<bool>("EnableEventLog"))
                    {
                        logging.AddEventLog(new EventLogSettings()
                        {
                            SourceName = "Mizekar.Micro.Idea",
                        });
                    }
                    logging.AddConsole();
                })
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
