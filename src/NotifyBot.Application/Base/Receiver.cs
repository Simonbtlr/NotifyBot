using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace NotifyBot.Application.Base;

public sealed class Receiver : IReceiver
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<Receiver> _logger;

    public Receiver(
        ITelegramBotClient telegramBotClient,
        IUpdateHandler updateHandler,
        ILogger<Receiver> logger)
    {
        _telegramBotClient = telegramBotClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken ct)
    {
        var opts = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true
        };

        var me = await _telegramBotClient.GetMeAsync(ct);
        
        _logger.LogInformation("Start receiving updates from {user}.", me.Username);

        await _telegramBotClient.ReceiveAsync(_updateHandler, opts, ct);
    }
}

public interface IReceiver
{
    Task ReceiveAsync(CancellationToken ct);
}