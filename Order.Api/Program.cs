using NLog;
using NLog.Web;
using Order.Api.Extensions;
using Order.Infrastructure.Data;
using WebFramework.Configuration;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    await WebApplication
        .CreateBuilder(args)
        .ConfigureServices()
        .Build()
        .Configure()
        .MigrateDatabase<OrderContext>((context, services) =>
        {
            var logger = services.GetService<ILogger<OrderContextSeed>>();
            OrderContextSeed.SeedAsync(context, logger).Wait();
        })
        .RunAsync();
}
catch (AggregateException ex)
{
    foreach (var innerException in ex.InnerExceptions)
    {
        logger.Error(innerException, "Unhandled exception in the application.");
    }
}

catch (Exception ex)
{
    logger.Error(ex);

    throw;
}

