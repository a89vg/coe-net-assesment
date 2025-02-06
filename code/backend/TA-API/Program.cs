using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Text;
using System.Text.Json.Serialization;
using TA_API;
using TA_API.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Is(LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
        .Enrich.FromLogContext();
    lc.WriteTo.Async(lc => lc.Console(new JsonFormatter()));
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddSwagger();

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var authConfig = new AuthConfig();
        builder.Configuration.GetSection("AuthConfig").Bind(authConfig);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = authConfig.Issuer,
            ValidAudience = authConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SigningKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser().AddRequirements(new RoleRequirement("Admin"));
    });
    options.AddPolicy("User", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.SetupApiServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSerilogRequestLogging();
app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
});
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthMiddleware();
app.MapControllers();

await app.RunAsync();
