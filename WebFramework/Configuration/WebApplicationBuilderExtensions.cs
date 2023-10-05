using Catalog.Application.Services;
using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities.Models;
using Catalog.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using System.Globalization;

namespace WebFramework.Configuration
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromFile("nlog.config")
                .GetCurrentClassLogger();

            try
            {
                if (builder is null)
                    throw new ArgumentNullException(nameof(builder));

                ConfigLogging(builder);

                var configuration = builder.Configuration;

                SetupNlog(builder);

                AddAppServices(builder);
                 

                AddMvcAndJsonOptions(builder);

                AddMinimalMvc(builder);

                AddCors(builder);


                AddAppHsts(builder);


#if !DEBUG
           //ApplyRemainingMigrations(builder); // TODO : بررسی بشه امکان اجرا در این حالت داره یا نه و گرنه باید به قسمت middleware برده بشه
#endif
                return builder;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }

        private static void AddAppServices(WebApplicationBuilder builder)
        { 
            builder.Services.AddTransient(typeof(IRepository<>), typeof(MongoDbRepository<>));
            //builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
            {
                var connectionString = "mongodb://localhost:27017/EShopping";
                var client = new MongoClient(connectionString);
                return client.GetDatabase("EShopping");
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }
        //private static void ApplyRemainingMigrations(WebApplicationBuilder builder)
        //{
        //    var serviceScopeFactory = builder.Services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        //    using (var serviceScope = serviceScopeFactory.CreateScope())
        //    {
        //        var dbContext = serviceScope.ServiceProvider.GetRequiredService<TavContext>();
        //        dbContext.Database.Migrate();
        //    }
        //}
        private static void SetupNlog(WebApplicationBuilder builder)
        {

            ILoggerFactory loggerFactory = new LoggerFactory();
            LogManager.Setup().LoadConfigurationFromFile("nlog.config");
            //loggerFactory.AddNLog().ConfigureNLog("nlog.config");
#if DEBUG
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddEventSourceLogger();
            builder.Logging.AddEventLog();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
#endif
            builder.Logging.ClearProviders();
            builder.Logging.AddNLogWeb();
            builder.Host.UseNLog();

        }

        private static void AddMvcAndJsonOptions(WebApplicationBuilder builder)
        {
            builder.Services
                             .AddControllers()
                             .AddJsonOptions(options =>
                             {
                                 options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                             })
                             .AddNewtonsoftJson(options =>
                             {
                                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                 options.SerializerSettings.Converters.Add(new StringEnumConverter());
                                 options.SerializerSettings.Culture = new CultureInfo("en");
                                 options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                                 options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                                 options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                                 options.SerializerSettings.ContractResolver = new DefaultContractResolver
                                 {
                                     NamingStrategy = new CamelCaseNamingStrategy()
                                 };
                                 options.AllowInputFormatterExceptionMessages = true;
                             });

        }
        private static void AddCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
        private static void AddAppHsts(WebApplicationBuilder builder)
        {
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                //options.ExcludedHosts.Add("example.com");
                //options.ExcludedHosts.Add("www.example.com");
            });
        }

        public static void AddMinimalMvc(WebApplicationBuilder builder)
        {
            //https://github.com/aspnet/AspNetCore/blob/0303c9e90b5b48b309a78c2ec9911db1812e6bf3/src/Mvc/Mvc/src/MvcServiceCollectionExtensions.cs
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter()); //Apply AuthorizeFilter as global filter to all actions

                //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
                //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
                //Use this filter when use cookie 
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //options.UseYeKeModelBinder();
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            //builder.Services.AddSwaggerGenNewtonsoftSupport();

            #region Old way (We don't need this from ASP.NET Core 3.0 onwards)
            ////https://github.com/aspnet/Mvc/blob/release/2.2/src/Microsoft.AspNetCore.Mvc/MvcServiceCollectionExtensions.cs
            //services.AddMvcCore(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter());

            //    //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
            //    //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
            //    //Use this filter when use cookie 
            //    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

            //    //options.UseYeKeModelBinder();
            //})
            //.AddApiExplorer()
            //.AddAuthorization()
            //.AddFormatterMappings()
            //.AddDataAnnotations()
            //.AddJsonOptions(option =>
            //{
            //    //option.JsonSerializerOptions
            //})
            //.AddNewtonsoftJson(/*option =>
            //{
            //    option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //    option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //}*/)

            ////Microsoft.AspNetCore.Mvc.Formatters.Json
            ////.AddJsonFormatters(/*options =>
            ////{
            ////    options.Formatting = Newtonsoft.Json.Formatting.Indented;
            ////    options.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            ////}*/)

            //.AddCors()
            //.SetCompatibilityVersion(CompatibilityVersion.Latest); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            #endregion
        }


        private static void ConfigLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddNLogWeb();
            builder.Host.UseNLog();
        }
    }
}
