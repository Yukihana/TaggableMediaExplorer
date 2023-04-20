using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TTX.Server.Startup;

namespace TTX.Server;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

        app.Run();
    }
}