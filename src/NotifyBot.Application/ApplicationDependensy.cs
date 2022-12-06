using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace NotifyBot.Application;

public static class ApplicationDependency
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddMediatR(typeof(ApplicationDependency).Assembly);
}