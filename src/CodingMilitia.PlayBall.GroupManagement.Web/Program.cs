using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Web.StartupHelpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;

namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("##### Starting my application! #####");

            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");

                var host = CreateWebHostBuilder(args).Build();

                await host.EnsureUpToDateAsync();

                host.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    logging.ClearProviders(); // clear default providers. NLog will handle everything
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //set default level to trace. NLog rules will kick in afterwards
                })
                .UseNLog()
                .UseStartup<Startup>();
    }
}
