using System;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using NotifyBot.Application.Base;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace NotifyBot.Modules;

public static class TelegramModule
{
    public static IServiceCollection AddTelegramModule(
        this IServiceCollection services)
    {
        var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (token is null)
            throw new ArgumentNullException(nameof(token));

        services.AddHttpClient("telegram-bot-client")
            .AddTypedClient<ITelegramBotClient>(httpClient =>
            {
                var opts = new TelegramBotClientOptions(token);

                return new TelegramBotClient(opts, httpClient);
            });

        services
            .AddScoped<IUpdateHandler, UpdateHandler>()
            .AddScoped<IReceiver, Receiver>()
            .AddHostedService<Polling>();

        return services;
    }
}