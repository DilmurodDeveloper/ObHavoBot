using ObHavoBot.Api.Models.Foundations.Weathers;

namespace ObHavoBot.Api.Services.Orchestrations.WeatherOrchestrations
{
    public interface IWeatherOrchestrationService
    {
        ValueTask<Weather> GetTodayWeatherAsync(double latitude, double longitude);
        ValueTask<Weather[]> GetWeeklyWeatherAsync(double latitude, double longitude);
    }
}
