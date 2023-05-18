using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using TTX.Server.Startup;

namespace TTX.Server;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            // Build the application
            WebApplication app = BuildTTX(args);

            // Run it here, not inside the method that built it.
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplication BuildTTX(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Attach logger
        builder.Host.UseSerilog();

        // Attach parameters
        string profilePath = builder.Configuration.GetProfilePath();
        RuntimeConfig runtimeConfig = builder.Services.AttachRuntimeConfig(profilePath);
        WorkspaceProfile workspaceProfile = builder.Services.AttachWorkspaceProfile(profilePath);

        // Add services to the container.
        builder.AttachAssetsDatabase(workspaceProfile, runtimeConfig);
        builder.Services.AttachDataServices();

        // Add core services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Build
        var app = builder.Build();

        app.InitializeServices();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // Return the app, so it can be run directly in the container block
        return app;
    }
}