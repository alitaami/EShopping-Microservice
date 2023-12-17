using NLog;
using NLog.Web;
using Order.Api.Extensions;
using Order.Infrastructure.Data;
using WebFramework.Configuration;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    WebApplication
        .CreateBuilder(args)
        .ConfigureServices()
        .Build()
        .Configure() 
        .MigrateDatabase<OrderContext>((context, services) =>
         {
             var logger = services.GetService<ILogger<OrderContextSeed>>();
             OrderContextSeed.SeedAsync(context, logger).Wait();
         })
        .Run();
}
catch (Exception ex)
{
    logger.Error(ex);

    throw;
}

