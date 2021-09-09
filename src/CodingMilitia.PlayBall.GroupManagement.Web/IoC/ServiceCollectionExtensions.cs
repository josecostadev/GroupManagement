﻿using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Data;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IGroupsService, GroupsService>();

            // add more

            return services;
        }

        public static IServiceCollection AddMisc(this IServiceCollection services)
        {
            //services.ConfigurePOCO<SomeRootConfiguration>(_config.GetSection("SomeRoot"));
            //services.ConfigurePOCO<SecretsConfiguration>(_config.GetSection("DemoSecrets"));

            services.AddTransient<RequestTimingFactoryMiddleware>();
            services.AddTransient<DemoExceptionFilter>();
            services.AddScoped<IGroupManagementDbContext, GroupManagementDbContext>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<GroupManagementDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("GroupManagementDbContext"), npgsqlOptions =>
                {
                    options.EnableSensitiveDataLogging();
                });
            });

            return services;
        }

        public static IServiceCollection AddRequiredMvcComponents(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvcCore();
            mvcBuilder.AddJsonFormatters();
            return services;
        }

        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}
