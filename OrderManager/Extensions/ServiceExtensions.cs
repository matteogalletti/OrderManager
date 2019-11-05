using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManager.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqliteContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["Connections:OrderDB"];
            services.AddDbContext<DomainContext>(opt => opt.UseSqlite(connectionString));
            //services.AddEntityFrameworkSqlite().AddDbContext<DomainContext>().ADDS;
        }
    }
}
