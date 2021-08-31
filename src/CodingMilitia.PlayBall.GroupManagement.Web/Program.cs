﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public static void Main(string[] args)
        {
            //Console.WriteLine("##### Starting my application! #####");
            //ConfigureNLog();
            //CreateWebHostBuilder(args).Build().Run();

            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
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

        // TODO: Replace with xml. Check https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-2
        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget("coloredConsole")
            {
                Layout = @"${date-format=HH\:mm\:ss} ${level} ${message} ${exception}",
            };

            var fileTarget = new FileTarget("file")
            {
                FileName = "${basedir}/web.log",
                Layout = @"${date-format=HH\:mm\:ss} ${level} ${message} ${exception} ${ndlc}"
            };

            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, consoleTarget, "CodingMilitia.PlayBall.GroupManagement.Web.IoC.*");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget);
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, fileTarget);

            config.AddTarget(consoleTarget);
            config.AddTarget(fileTarget);
            LogManager.Configuration = config;
        }
    }
}
