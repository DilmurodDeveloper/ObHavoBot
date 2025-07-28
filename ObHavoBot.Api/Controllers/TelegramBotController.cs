using Microsoft.AspNetCore.Mvc;
using ObHavoBot.Api.Services.Processings.TelegramMessages;
using RESTFulSense.Controllers;
using Telegram.Bot.Types;

namespace ObHavoBot.Api.Controllers
{
    [ApiController]
    [Route("api/telegram/update")]
    public class TelegramBotController : RESTFulController
    {
        private readonly ITelegramMessageProcessingService telegramMessageProcessingService;

        public TelegramBotController(
            ITelegramMessageProcessingService telegramMessageProcessingService)
        {
            this.telegramMessageProcessingService = telegramMessageProcessingService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> PostUpdateAsync(Update update)
        {
            await telegramMessageProcessingService.ProcessUpdateAsync(update);
            return Ok();
        }
    }
}
