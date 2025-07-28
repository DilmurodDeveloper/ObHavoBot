namespace ObHavoBot.Api.Models.Foundations.Telegrams
{
    public class TelegramUpdate
    {
        public long UpdateId { get; set; }
        public TelegramMessage Message { get; set; }
        public TelegramCallbackQuery CallbackQuery { get; set; }
    }
}
