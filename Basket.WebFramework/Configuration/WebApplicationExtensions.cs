﻿using HealthChecks.UI.Client;
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
                // If you're in a development environment, you might want to see detailed exception information.
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwaggerAndUI();
                }
                else
                {
                    // Use HTTPS Redirection in non-development environments
                    app.UseHttpsRedirection();
                }
                app.UseRouting();
                app.UseCors();
                //app.UseAuthentication();
                app.UseStaticFiles();
                //app.UseAuthorization();


                // Endpoints mapping comes after Authentication and Authorization.
                app.MapControllers();

                app.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

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
