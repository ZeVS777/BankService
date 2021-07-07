using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SovComBankTest.ApiWebApp.Models;
using SovComBankTest.Services.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SovComBankTest.ApiWebApp
{
    internal class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddSmsService()
                .AddVersionedApiExplorer(ConfigureApiExplorerOptions)
                .AddApiVersioning(ConfigureApiVersionOptions)
                .AddSwaggerGen(ConfigureSwaggerGenOptions)
                .AddControllers(ConfigureMvcOptions).ConfigureApiBehaviorOptions(ConfigureApiBehaviorOptions);

        public void Configure(IApplicationBuilder app) => app
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                            Description = "Основной сервер"
                        }
                    };
                });
            })
            .UseSwaggerUI(setup =>
            {
                foreach (var existingVersion in VersionConfig.ExistingVersions)
                {
                    setup.SwaggerEndpoint($"/swagger/{existingVersion}/swagger.json",
                        existingVersion);
                }

                setup.RoutePrefix = "help";
                setup.EnableValidator();
                setup.DocumentTitle = "Web-сервис для отправки текстового приглашения в виде SMS-сообщения со ссылкой для установки приложения на телефоны незарегистрированных пользователей Системы.";
            })
            .UseExceptionHandler("/error")
            .UseStatusCodePagesWithReExecute("/error/{0}")
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        private static Action<ApiBehaviorOptions> ConfigureApiBehaviorOptions => options =>
        {
            options.SuppressMapClientErrors = true;
            options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(
                //Здесь мы можем оценить context и на основе конкретной ошибки предоставить код, если требуется
                new ApiResult<IEnumerable<ValidationError>>(ValidationError.GetValidationErrors(context),
                    "Bad Request"));
        };

        private static Action<ApiExplorerOptions> ConfigureApiExplorerOptions => options =>
        {
            options.DefaultApiVersion = VersionConfig.DefaultApiVersion;
            options.SubstituteApiVersionInUrl = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        };

        private static Action<ApiVersioningOptions> ConfigureApiVersionOptions => options =>
        {
            options.DefaultApiVersion = VersionConfig.DefaultApiVersion;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader(VersionConfig.ApiVersionParameterName),
                new HeaderApiVersionReader(VersionConfig.ApiVersionParameterName)
            );
            options.AssumeDefaultVersionWhenUnspecified = true;
        };

        private static Action<SwaggerGenOptions> ConfigureSwaggerGenOptions => swaggerGenOptions =>
        {
            swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"), true);
            
            AddSwaggerApiVersionDocs(swaggerGenOptions);
            swaggerGenOptions.UseAllOfToExtendReferenceSchemas();


            void AddSwaggerApiVersionDocs(SwaggerGenOptions swaggerGenOptions)
            {
                foreach (var existingVersion in VersionConfig.ExistingVersions)
                {
                    swaggerGenOptions.SwaggerDoc(existingVersion, new OpenApiInfo
                    {
                        Title = "Web-сервис для отправки текстового приглашения.",
                        Version = existingVersion,
                        Contact = new OpenApiContact
                        {
                            Url = new Uri("https://sovcombank.ru/"),
                            Name = "Совкомбанк",
                            Email = "help@example.com"
                        },
                        Description = "Web-сервис для отправки текстового приглашения в виде SMS-сообщения со ссылкой для установки приложения на телефоны незарегистрированных пользователей Системы.",
                        License = new OpenApiLicense
                        {
                            Url = new Uri("https://ru.wikipedia.org/wiki/%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F_MIT"),
                            Name = "Лицензия"
                        }
                    });
                }
            }
        };

        private static Action<MvcOptions> ConfigureMvcOptions => options =>
        {
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult<ErrorFeatures>), StatusCodes.Status500InternalServerError));
        };
    }

    /// <summary>
    ///     Конфигурация версионности API
    /// </summary>
    internal class VersionConfig
    {
        /// <summary>
        ///     Параметр в запросе или заголовке, ответственный за назначение версии запрашиваемого API
        /// </summary>
        public const string ApiVersionParameterName = "api-version";

        /// <summary>
        ///     Версия API по умолчанию
        /// </summary>
        public static readonly ApiVersion DefaultApiVersion = new(1, 0);

        /// <summary>
        ///     Активные версии АПИ
        /// </summary>
        public static readonly string[] ExistingVersions = {"1.0"};
    }
}
