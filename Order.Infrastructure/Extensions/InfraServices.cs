using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Core.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Extensions
{
    public static class InfraServices
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Additional code...

            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(OrderContext).Assembly.GetName().Name);

                    //Configuring Connection Resiliency:
                    sqlOptions.
                        EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
                 
            });

            services
                .AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>))
                .AddScoped<IOrderRepository, OrderRepository>();

            return services;
        } 
    }
}
