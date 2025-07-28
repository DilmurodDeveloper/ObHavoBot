using Telegram.Bot.Types;

namespace ObHavoBot.Api.Services.Processings.TelegramMessages
{
    public interface ITelegramMessageProcessingService
    {
        ValueTask ProcessUpdateAsync(Update update);
    }
}
