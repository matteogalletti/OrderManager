using System;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManager.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqliteContext(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<DomainContext>();
        }
    }
}
