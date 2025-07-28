namespace ObHavoBot.Api.Models.Foundations.Telegrams
{
    public class TelegramMessage
    {
        public long MessageId { get; set; }
        public TelegramUser From { get; set; }
        public long Date { get; set; }
        public string Text { get; set; }
        public TelegramChat Chat { get; set; }
        public TelegramLocation Location { get; set; }
    }

    public class TelegramUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
    }

    public class TelegramChat
    {
        public long Id { get; set; }
        public string Type { get; set; }
    }
}
