using ObHavoBot.Api.Models.Foundations.Weathers;

namespace ObHavoBot.Api.Services.Foundations.Weathers
{
    public interface IWeatherService
    {
        ValueTask<Weather> RetrieveTodayWeatherByCoordinatesAsync(double latitude, double longitude);
        ValueTask<Weather[]> RetrieveWeeklyWeatherForecastAsync(double latitude, double longitude);
    }
}
