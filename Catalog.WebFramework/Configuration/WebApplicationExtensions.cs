
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using NLog;
using WebFramework.Configuration.Swagger;

namespace WebFramework.Configuration
{
    public static class WebApplicationExtensions
    {

        public static WebApplication Configure(this WebApplication app)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

            try
            {
                //app.UseNAPCustomHeadersMidleware();
                //app.UseNAPExceptionHandlerMiddleware();
                app.UseHttpsRedirection();
                app.UseHsts();
                app.UseCors();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.MapHealthChecks("/health" , new HealthCheckOptions()
                {
                    Predicate = _ => true ,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
               
                //app.MapHangfireDashboard("/hangfire");
                //app.UseHangfireDashboard();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerAndUI();
                }
                return app;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
}
    }
}