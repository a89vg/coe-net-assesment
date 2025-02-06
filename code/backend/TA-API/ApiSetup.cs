using Microsoft.OpenApi.Models;
using System.Reflection;
using TA_API.Interfaces;
using TA_API.Services.Data;
using TA_API.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using TA_API.Filters;
using Microsoft.AspNetCore.Authorization;
using TA_API.Auth;
using FluentValidation;
using TA_API.Models;
using TA_API.Validation;

namespace TA_API;

public static class ApiSetup
{

    public static TSettings AddSettings<TSettings>(this IServiceCollection services, IConfiguration configuration) where TSettings : class, new()
    {
        var settings = new TSettings();
        configuration.Bind(settings);
        services.AddSingleton(settings);
        return settings;
    }

    public static void SetupApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSettings<AuthConfig>(config.GetSection("AuthConfig"));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetSection("CacheConfig")["EndpointUrl"];
        });

        services.AddScoped(provider => new CurrentUserSessionProvider());
        services.AddScoped<SessionProviderMiddleware>();
        services.AddScoped<IAuthorizationHandler, ApiAuthHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthFailureHandler>();

        services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(config.GetConnectionString("AssessmentDB")));
        services.AddScoped<IValidator<NewUserModel>, NewUserValidator>();
        services.AddScoped<IValidator<UserUpdateModel>, UserUpdateValidator>();
        services.AddScoped<IValidator<UserLoginModel>, UserLoginValidator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<ErrorHandlingFilter>();
        services.AddHttpClient();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.Name);
            options.SelectDiscriminatorNameUsing((baseType) => "TypeName");
            options.SelectDiscriminatorValueUsing((subType) => subType.Name);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            var openApiInfo = new OpenApiInfo
            {
                Title = "CoE .NET Assesment",
                Description = string.Empty,
                Version = "1.0"
            };

            options.SwaggerDoc("1.0", openApiInfo);
        });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"1.0/swagger.json", "CoE .NET Assesment");
            options.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
            options.RoutePrefix = "swagger";
        });
    }
}
