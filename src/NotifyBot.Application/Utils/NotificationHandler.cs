using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Bot;

namespace NotifyBot.Application.Utils;

public abstract class NotificationHandler<TEvent> 
    : INotificationHandler<TEvent>
    where TEvent : INotification
{
    protected ITelegramBotClient TelegramBotClient;

    protected NotificationHandler(ITelegramBotClient telegramBotClient)
    {
        TelegramBotClient = telegramBotClient;
    }

    public async Task Handle(TEvent messageEvent, CancellationToken ct) =>
        await HandleAsync(messageEvent, ct);

    public abstract Task HandleAsync(TEvent messageEvent, CancellationToken ct);
}