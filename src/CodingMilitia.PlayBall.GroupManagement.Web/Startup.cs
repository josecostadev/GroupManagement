using CodingMilitia.PlayBall.GroupManagement.Data;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[assembly: ApiController]
namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRequiredMvcComponents();

            // Removed the default DI container
            services.AddBusiness();
            services.AddMisc();
            services.AddDatabase(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // default is this, but we can do it differently: https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-2.2
                app.UseDeveloperExceptionPage();
            }

            // Middleware to serve static files - Short Circuit
            app.UseStaticFiles();

            AddPoweredByHeaderMiddleware(app);

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("No middlewares could handle the request");
            });
        }

        private static void AddTestMiddlewares(IApplicationBuilder app)
        {
            // Map when
            app.MapWhen(context => context.Request.Headers.Keys.Any(o => o.StartsWith("map")), builder =>
            {
                builder.UseMiddleware<RequestTimingFactoryMiddleware>();
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("pong from \"map when\" branch");
                });
            });

            // Map
            app.Map("/ping", builder =>
            {
                builder.UseMiddleware<RequestTimingFactoryMiddleware>();
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("pong from map branch");
                });
            });

            app.UseMiddleware<RequestTimingAdhocMiddleware>();
        }

        private static void AddPoweredByHeaderMiddleware(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // OnStarting because if we write directly to the response
                // it can already being streamed to the client.
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("X-Powered-By"))
                    { 
                        context.Response.Headers.Add("X-Powered-By", "ASP.Net Core: From 0 to overkill.");
                    }

                    return Task.CompletedTask;
                });

                await next.Invoke();
            });
        }
    }
}
