using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Interfaces;
using TA_API.Services;
using TA_API.Services.Data;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));
}

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
        
}

app.Run();
