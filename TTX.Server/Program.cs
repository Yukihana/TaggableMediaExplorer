using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using TTX.Server.Startup;

namespace TTX.Server;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Load Config
        var profile = builder.Configuration.LoadWorkspaceProfile();
        if (profile == null)
            throw new InvalidDataException("Unable to load profile. If this is a first run, then a new profile would have been created. Please run the program again after configuring the values in the profile.");

        // Add services to the container.
        builder.SetupDatabase(profile.ServerRoot);
        builder.Services.AttachDataServices(profile);

        // Add core services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Initialize (verify if here or later)
        // send start database load (it'll load to dormant list)
        // send first scan to acquiservice (items will validate and move db entries to live list)

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}