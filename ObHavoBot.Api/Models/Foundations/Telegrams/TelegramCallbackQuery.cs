namespace ObHavoBot.Api.Models.Foundations.Telegrams
{
    public class TelegramCallbackQuery
    {
        public string Id { get; set; }
        public TelegramUser From { get; set; }
        public TelegramMessage Message { get; set; }
        public string Data { get; set; }
    }
}
