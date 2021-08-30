using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // old configuration registration
            //services.Configure<SomeRootConfiguration>(_configuration.GetSection("SomeRoot"));

            // register a singleton populated from the config
            //var someRootConfiguration = new SomeRootConfiguration();
            //_config.GetSection("SomeRoot").Bind(someRootConfiguration);
            //services.AddSingleton(someRootConfiguration);

            services.ConfigurePOCO<SomeRootConfiguration>(_config.GetSection("SomeRoot"));
            services.ConfigurePOCO<SecretsConfiguration>(_config.GetSection("DemoSecrets"));

            // Removed the default DI container
            //services.AddBusiness();

            // Add services to the collection
            services.AddOptions();

            // Create a container-builder and register dependencies
            var containerBuilder = new ContainerBuilder();

            // Register your own things directly with Autofac
            containerBuilder.RegisterModule<AutofacModule>();

            // Populate the service-descriptors added to `IServiceCollection`
            // BEFORE you add things to Autofac so that the Autofac
            // registrations can override stuff in the `IServiceCollection`
            // as needed
            containerBuilder.Populate(services);

            var autofacContainer = containerBuilder.Build();

            // this will be used as the service-provider for the application!
            return new AutofacServiceProvider(autofacContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            // Middleware to serve static files
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
