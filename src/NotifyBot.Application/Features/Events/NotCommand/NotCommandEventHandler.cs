
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NotifyBot.Application.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyBot.Application.Features.Events.NotCommand;

public sealed class NotCommandEventHandler : NotificationHandler<NotCommandEvent>
{
    private const string Message = "I'm sorry, I don't understand anything. Try typing /help";
    
    private readonly ILogger<NotCommandEventHandler> _logger;

    public NotCommandEventHandler(ITelegramBotClient telegramBotClient, ILogger<NotCommandEventHandler> logger) 
        : base(telegramBotClient)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(NotCommandEvent messageEvent, CancellationToken ct)
    {
        var chatId = new ChatId(messageEvent.ChatId);

        _logger.LogInformation(
            "In chat {chatId} {firstName} is trying to talk to the bot, but the bot doesn't understand him.\n" +
            "Text: {text}\n" +
            "MessageType: {messageType}",
            chatId,
            messageEvent.FirstName ?? "someone",
            messageEvent.Text ?? "null",
            messageEvent.MessageType);
        
        await TelegramBotClient.SendTextMessageAsync(
            chatId,
            Message,
            cancellationToken: ct);
    }
}