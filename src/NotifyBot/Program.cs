using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NotifyBot;
using Serilog;

var host = Host.CreateDefaultBuilder()
    .UseSerilog()
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build();
    
await host.RunAsync();