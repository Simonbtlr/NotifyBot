using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using NotifyBot.Application.Features.Events.NotCommand;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotifyBot.Application.Base;

public sealed class UpdateHandler : IUpdateHandler
{
    private const int RetrySeconds = 5;
    
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateHandler(ILogger<UpdateHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken ct)
    {
        _logger.LogInformation("Receive {updateType} update.", update.Type);

        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleUpdateTypeMessage(update, ct);
                break;
            case UpdateType.Unknown:
            default:
                await HandleUpdateTypeUnknown(update, ct);
                break;
        }
    }

    public async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken ct)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n" +
                                                       $"[{apiRequestException.ErrorCode}]\n" +
                                                       $"{apiRequestException.Message}.",
            _ => exception.ToString()
        };
        
        _logger.LogError("Handle Error: {error}", errorMessage);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(RetrySeconds), ct);
    }

    #region Handlers

    private async Task HandleUpdateTypeMessage(Update update, CancellationToken ct)
    {
        if (update is not {Message: { } message})
            throw new ArgumentNullException(nameof(message));
        
        _logger.LogInformation("Receive {messageType} message type.", message.Type);

        switch (message.Text?.Split(' ')[0])
        {
            default:
                await HandleNotCommand();
                break;
        }

        async Task HandleNotCommand()
        {
            var messageEvent = new NotCommandEvent(
                message.Chat.Id,
                message.Chat.FirstName,
                message.Text,
                message.Type);

            await _mediator.Publish(messageEvent, ct);
        }
    }

    private Task HandleUpdateTypeUnknown(Update update, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
    
    #endregion
}