using Discount.API;
using Discount.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using NLog.Web;

public class Program
{
    [Obsolete]
    public static void Main(string[] args)
    {
        var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        try
        {
            logger.Info("Initializing application...");
            var host = CreateHostBuilder(args).Build();

            // Perform the database migration
            logger.Info("Starting database migration...");
            host.MigrateDatabase<Program>();
            logger.Info("Database migration completed successfully.");

            // Run the application
            host.Run();
        }
        catch (Exception ex)
        {
            // NLog: catch setup errors
            logger.Error(ex, "Stopped program because of exception during startup.");
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application exit (Avoid segmentation fault on Linux)
            NLog.LogManager.Shutdown();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseNLog(); // NLog: Setup NLog for Dependency injection
}
