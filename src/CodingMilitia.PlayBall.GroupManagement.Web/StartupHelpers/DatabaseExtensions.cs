using CodingMilitia.PlayBall.GroupManagement.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.StartupHelpers
{
    public static class DatabaseExtensions
    {
        public static async Task EnsureUpToDateAsync(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IGroupManagementDbContext>();
                await context.DataContext.Database.MigrateAsync();
            }
        }
    }
}
