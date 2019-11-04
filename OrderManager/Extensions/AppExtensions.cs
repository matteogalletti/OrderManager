using System;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManager.Extensions
{
    public static class AppExtensions
    {
        public static void InitializeDb(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var ctx = serviceScope.ServiceProvider.GetRequiredService<DomainContext>();
                ctx.Database.EnsureCreated();
            }
        }
    }
}
