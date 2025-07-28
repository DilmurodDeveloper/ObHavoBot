using ObHavoBot.Api.Models.Foundations.Weathers;

namespace ObHavoBot.Api.Brokers.Weathers
{
    public interface IWeatherBroker
    {
        ValueTask<Weather> GetTodayWeatherByCoordinatesAsync(double lat, double lon);
        ValueTask<List<Weather>> Get5Day3HourForecastByCoordinatesAsync(double lat, double lon);
    }
}
