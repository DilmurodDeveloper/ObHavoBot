namespace ObHavoBot.Api.Brokers.Telegrams
{
    public interface ITelegramBotBroker
    {
        ValueTask SendTextMessageAsync(long chatId, string text);
        ValueTask SendLocationRequestMessageAsync(long chatId);
        ValueTask SendWeatherTypeMenuAsync(long chatId, double latitude, double longitude);
    }
}
