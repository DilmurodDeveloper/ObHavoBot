using System.Text;
using System.Text.Json;

namespace ObHavoBot.Api.Brokers.Telegrams
{
    public class TelegramBotBroker : ITelegramBotBroker
    {
        private readonly HttpClient httpClient;
        private readonly string botToken;
        private readonly string telegramApiUrl;

        public TelegramBotBroker(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.botToken = configuration["TelegramBot:Token"];
            this.telegramApiUrl = $"https://api.telegram.org/bot{botToken}";
        }

        public async ValueTask SendTextMessageAsync(long chatId, string text)
        {
            var message = new
            {
                chat_id = chatId,
                text = text
            };

            string content = JsonSerializer.Serialize(message);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await httpClient.PostAsync($"{telegramApiUrl}/sendMessage", httpContent);
        }

        public async ValueTask SendLocationRequestMessageAsync(long chatId)
        {
            var message = new
            {
                chat_id = chatId,
                text = "📍 Iltimos, joylashuvingizni yuboring",
                reply_markup = new
                {
                    keyboard = new[]
                    {
                        new[]
                        {
                            new
                            {
                                text = "📍 Joylashuvni yuborish",
                                request_location = true
                            }
                        }
                    },
                    resize_keyboard = true,
                    one_time_keyboard = true
                }
            };

            string content = JsonSerializer.Serialize(message);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await httpClient.PostAsync($"{telegramApiUrl}/sendMessage", httpContent);
        }

        public async ValueTask SendWeatherTypeMenuAsync(long chatId, double latitude, double longitude)
        {
            var callbackDataToday = $"today:{latitude}:{longitude}";
            var callbackDataWeek = $"week:{latitude}:{longitude}";

            var message = new
            {
                chat_id = chatId,
                text = "Qaysi turdagi ma'lumot kerak?",
                reply_markup = new
                {
                    inline_keyboard = new[]
                    {
                        new[]
                        {
                            new
                            {
                                text = "🌤 Bugungi ob-havo",
                                callback_data = callbackDataToday
                            },
                            new
                            {
                                text = "📅 7 kunlik prognoz",
                                callback_data = callbackDataWeek
                            }
                        }
                    }
                }
            };

            string content = JsonSerializer.Serialize(message);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await httpClient.PostAsync($"{telegramApiUrl}/sendMessage", httpContent);
        }
    }
}
