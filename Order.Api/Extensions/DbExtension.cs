using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using Order.Infrastructure.Data;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");

                    // Drop the table if it exists
                    // Create and Seed data using raw SQL query for orders
                    context.Database.ExecuteSqlRaw(OrderDataSeeder.SeedQuery(typeof(TContext).Name));
                     
                    logger.LogInformation($"Migration and Data Seeding has Completed: {typeof(TContext).Name}");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"An error occurred while migrating db: {typeof(TContext).Name}");
                }
            }

            return host;
        }
    }
}
