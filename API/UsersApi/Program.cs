#pragma warning disable SA1200 // Using directives should be placed correctly
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UsersApi.ErrorHandling;
using UsersApi.Extensions;
using UsersApi.Models;
#pragma warning restore SA1200 // Using directives should be placed correctly

try
{
    Log.Information("Starting Users API");

    var builder = WebApplication.CreateBuilder(args);

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    builder.Services.AddDbContext<UserContext>(options =>
    {
        options.UseNpgsql(builder.Configuration["ConnectionStrings:Users"]);
        options.EnableDetailedErrors();

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    });

    builder.Services.InjectDependencies();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // allow everything
    builder.Services.AddCors(
        options =>
            options.AddPolicy(
                "CorsPolicy",
                builder =>
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
            )
    );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("CorsPolicy");

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseAuthorization();

    app.MapControllers();
    app.MapGet("/ping", [AllowAnonymous] () => Task.CompletedTask);

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Api start up error");
}
finally
{
    Log.Information("Api shut down");
    Log.CloseAndFlush();
}