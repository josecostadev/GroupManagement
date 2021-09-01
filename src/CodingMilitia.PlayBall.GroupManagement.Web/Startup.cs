using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddMvc(options =>
            {
                options.Filters.Add<DemoActionFilter>();
            });

            RegisterDependencies(services);

            // Removed the default DI container
            services.AddBusiness();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Middleware to serve static files - Short Circuit
            app.UseStaticFiles();

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
            app.Map("/ping", builder => { 
                builder.UseMiddleware<RequestTimingFactoryMiddleware>();
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("pong from map branch");
                });
            });

            //app.UseMiddleware<RequestTimingAdhocMiddleware>();

            app.Use(async (context, next) =>
            {
                // OnStarting because if we write directly to the response
                // it can already being streamed to the client.
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("X-Powered-By", "ASP.Net Core: From 0 to overkill.");
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("No middlewares could handle the request");
            });
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            //services.ConfigurePOCO<SomeRootConfiguration>(_config.GetSection("SomeRoot"));
            //services.ConfigurePOCO<SecretsConfiguration>(_config.GetSection("DemoSecrets"));

            services.AddScoped<RequestTimingFactoryMiddleware>();
            services.AddTransient<DemoExceptionFilter>();
        }
    }
}
