using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotifyBot.Modules;
using Serilog;

namespace NotifyBot;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLoggingModule(_configuration)
            ;
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseSerilogRequestLogging();
    }
}