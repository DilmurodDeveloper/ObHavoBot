using System.Text.RegularExpressions;
using ObHavoBot.Api.Brokers.Telegrams;
using ObHavoBot.Api.Brokers.Weathers;
using Telegram.Bot.Types;

namespace ObHavoBot.Api.Services.Processings.TelegramMessages
{
    public class TelegramMessageProcessingService : ITelegramMessageProcessingService
    {
        private readonly ITelegramBotBroker telegramBotBroker;
        private readonly IWeatherBroker weatherBroker;

        public TelegramMessageProcessingService(
            ITelegramBotBroker telegramBotBroker,
            IWeatherBroker weatherBroker)
        {
            this.telegramBotBroker = telegramBotBroker;
            this.weatherBroker = weatherBroker;
        }

        public async ValueTask ProcessUpdateAsync(Update update)
        {
            if (update.Message is not null)
                await ProcessMessageAsync(update.Message);

            if (update.CallbackQuery is not null)
                await ProcessCallbackQueryAsync(update.CallbackQuery);
        }

        private async ValueTask ProcessMessageAsync(Message message)
        {
            long chatId = message.Chat.Id;

            if (message.Text == "/start")
            {
                await telegramBotBroker.SendTextMessageAsync(chatId,
                    "Assalomu alaykum! Ob-havo botiga xush kelibsiz 👋");

                await telegramBotBroker.SendLocationRequestMessageAsync(chatId);
            }

            else if (message.Location is not null)
            {
                double lat = message.Location.Latitude;
                double lon = message.Location.Longitude;

                await telegramBotBroker.SendWeatherTypeMenuAsync(chatId, lat, lon);
            }
        }

        private async ValueTask ProcessCallbackQueryAsync(CallbackQuery callbackQuery)
        {
            long chatId = callbackQuery.Message.Chat.Id;
            string data = callbackQuery.Data;

            // Format: "today:41.3:69.2" yoki "week:41.3:69.2"
            Match match = Regex.Match(data, @"^(today|week):([0-9.]+):([0-9.]+)$");

            if (match.Success)
            {
                string type = match.Groups[1].Value;
                double lat = double.Parse(match.Groups[2].Value);
                double lon = double.Parse(match.Groups[3].Value);

                if (type == "today")
                {
                    var weather = await weatherBroker.GetTodayWeatherByCoordinatesAsync(lat, lon);

                    string messageText = $"📍 Shahar: {weather.City}\n" +
                                         $"📅 Sana: {weather.Date:dd.MM.yyyy}\n" +
                                         $"🌡 Harorat: {weather.Temperature}°C\n" +
                                         $"🌥 Tavsif: {weather.Description}";

                    await telegramBotBroker.SendTextMessageAsync(chatId, messageText);
                }

                if (type == "week")
                {
                    var weathers = await weatherBroker.Get7DayForecastByCoordinatesAsync(lat, lon);

                    string messageText = "📅 7 kunlik ob-havo prognozi:\n\n";

                    foreach (var weather in weathers)
                    {
                        messageText += $"📆 {weather.Date:dd.MM.yyyy}: " +
                                       $"{weather.Temperature}°C, " +
                                       $"{weather.Description}\n";
                    }

                    await telegramBotBroker.SendTextMessageAsync(chatId, messageText);
                }
            }
        }
    }
}
