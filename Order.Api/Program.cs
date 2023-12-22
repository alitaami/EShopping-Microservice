using NLog;
using NLog.Web;
using Order.Infrastructure.Data;
using Ordering.API.Extensions;
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

