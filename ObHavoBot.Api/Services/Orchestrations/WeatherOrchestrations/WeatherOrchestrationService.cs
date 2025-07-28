using ObHavoBot.Api.Models.Foundations.Weathers;
using ObHavoBot.Api.Services.Foundations.Weathers;

namespace ObHavoBot.Api.Services.Orchestrations.WeatherOrchestrations
{
    public class WeatherOrchestrationService : IWeatherOrchestrationService
    {
        private readonly IWeatherService weatherService;

        public WeatherOrchestrationService(IWeatherService weatherService) =>
            this.weatherService = weatherService;

        public ValueTask<Weather> GetTodayWeatherAsync(double latitude, double longitude) =>
            this.weatherService.RetrieveTodayWeatherByCoordinatesAsync(latitude, longitude);

        public ValueTask<Weather[]> GetWeeklyWeatherAsync(double latitude, double longitude) =>
            this.weatherService.RetrieveWeeklyWeatherForecastAsync(latitude, longitude);
    }
}
