using MediatR;
using Telegram.Bot.Types.Enums;

namespace NotifyBot.Application.Features.Events.NotCommand;

public sealed record NotCommandEvent(
    long ChatId,
    string? FirstName,
    string? Text,
    MessageType MessageType) 
    : INotification;