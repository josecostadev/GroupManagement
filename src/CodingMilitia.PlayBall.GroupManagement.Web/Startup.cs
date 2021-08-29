using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Dependency Injection (ony by one)
            //services.AddSingleton<IGroupsService, InMemoryGroupsService>();

            services.AddBusiness();
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
