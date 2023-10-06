using Catalog.Application.Services;
using Catalog.Application.Services.Interfaces;
using Catalog.Core.Entities;
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
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Common.Utilities;
using Microsoft.OpenApi.Models;
using WebFramework.Configuration.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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

                AddCustomApiVersioning(builder);
                
                AddSwagger(builder);

                AddHealthChecks(builder);

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

        public static void AddHealthChecks(WebApplicationBuilder builder)
        {
            var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDB");

            builder.Services.AddHealthChecks()
        .AddMongoDb(
            mongodbConnectionString: mongoDbConnectionString,
            name: "mongodb",
            failureStatus: HealthStatus.Degraded
        );
        }

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            Assert.NotNull(builder.Services, nameof(builder.Services));

            //Add services to use Example Filters in swagger
            //services.AddSwaggerExamples();
            //Add services and configuration to use swagger
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "MyApi.xml");
                //show controller XML comments like summary
                options.IncludeXmlComments(xmlDocPath, true);
                //options.OperationFilter<FormFileSwaggerFilter>();
                //options.EnableAnnotations();
                options.UseInlineDefinitionsForEnums();
                //options.DescribeAllParametersInCamelCase();
                //options.DescribeStringEnumsInCamelCase();
                //options.UseReferencedDefinitionsForEnums();
                //options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //options.IgnoreObsoleteActions();
                //options.IgnoreObsoleteProperties();
                //options.CustomSchemaIds(type => type.FullName);

                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });
                //options.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "API V2" });

                #region Filters
                ////Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                //options.ExampleFilters();

                ////Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                options.OperationFilter<FileUploadOperation>(); // Add this line to enable file upload
                                                                ////Set summary of action if not already set
                options.OperationFilter<ApplySummariesOperationFilter>();

                //#region Add UnAuthorized to Response
                ////Add 401 response and security requirements (Lock icon) to actions that need authorization
                options.OperationFilter<UnauthorizedResponsesOperationFilter>(false, "Bearer");
                //#endregion

                #region security for swagger

                //        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //        {
                //            In = ParameterLocation.Header,
                //            Description = "Please enter a valid token",
                //            Name = "Authorization",
                //            Type = SecuritySchemeType.Http,
                //            BearerFormat = "JWT",
                //            Scheme = "Bearer"
                //        });
                //        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type=ReferenceType.SecurityScheme,
                //                Id="Bearer"
                //            }
                //        },
                //        new string[]{}
                //    }
                //});

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:7076/api/Account/Login"),
                            Scopes = new Dictionary<string, string>
            {
                {"read", "Read access to protected resources."},
                {"write", "Write access to protected resources."},
            }
                        }
                    }
                });

                #endregion

                //#region Add Jwt Authentication
                //Add Lockout icon on top of swagger ui page to authenticate

                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "bearer"
                //});

                ////options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                ////{
                ////    {"Bearer", new string[] { }}
                ////});
                ///
                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Implicit = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("https://localhost:7188/api/v1/User/login"),
                //            Scopes = new Dictionary<string, string>
                //    {
                //        {"read", "Read access to protected resources."},
                //        {"write", "Write access to protected resources."},
                //    }
                //        }
                //    }
                //});
                #endregion

                #region Versioning
                // Remove version parameter from all Operations
                options.OperationFilter<RemoveVersionParameters>();

                ////set version "api/v{version}/[controller]" from current swagger doc verion
                options.DocumentFilter<SetVersionInPaths>();

                ////Seperate and categorize end-points by doc version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
                #endregion

                //If use FluentValidation then must be use this package to show validation in swagger (MicroElements.Swashbuckle.FluentValidation)
                //options.AddFluentValidationRules();

            });
        }


        private static void AddCustomApiVersioning(WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;

                ApiVersion.TryParse("1.0", out var version10);
                ApiVersion.TryParse("1", out var version1);
                var a = version10 == version1; // Note: This is unused. Do you need it?

                // Uncomment the version reader you wish to use. Do not set multiple, 
                // as each one will overwrite the previous one.

                // Use query string for versioning e.g., api/posts?api-version=1
                // options.ApiVersionReader = new QueryStringApiVersionReader("api-version"); 

                // Use URL segment for versioning e.g., api/v1/posts
                // options.ApiVersionReader = new UrlSegmentApiVersionReader(); 

                // Use header for versioning e.g., Header => Api-Version: 1
                // options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" }); 

                // Use MediaType for versioning (this requires more setup)
                // options.ApiVersionReader = new MediaTypeApiVersionReader();

                // Use a combination of query string and URL segment
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new UrlSegmentApiVersionReader()
                );
            });
        }

        private static void AddAppServices(WebApplicationBuilder builder)
        {
            // 1. IMongoDatabase registration
            builder.Services.AddScoped<IMongoDatabase>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MongoDB");
                var databaseName = configuration["MongoDatabaseName"] ?? "Eshopping";

                var mongoClient = new MongoClient(connectionString);
                return mongoClient.GetDatabase(databaseName);
            });

            // 2. MongoDbContext registration
            builder.Services.AddScoped<MongoDbContext>();

            // 4. Existing registrations
            builder.Services.AddTransient(typeof(IRepository<>), typeof(MongoDbRepository<>));
            builder.Services.AddTransient<IProductService, ProductService>();

            // 5. Other services
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
