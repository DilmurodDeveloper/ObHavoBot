using ObHavoBot.Api.Brokers.Weathers;
using ObHavoBot.Api.Models.Foundations.Weathers;

namespace ObHavoBot.Api.Services.Foundations.Weathers
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherBroker weatherBroker;

        public WeatherService(IWeatherBroker weatherBroker) =>
            this.weatherBroker = weatherBroker;

        public ValueTask<Weather> RetrieveTodayWeatherByCoordinatesAsync(double latitude, double longitude) =>
            this.weatherBroker.GetTodayWeatherByCoordinatesAsync(latitude, longitude);

        public async ValueTask<Weather[]> RetrieveWeeklyWeatherForecastAsync(double latitude, double longitude)
        {
            List<Weather> weeklyWeatherList =
                await this.weatherBroker.Get7DayForecastByCoordinatesAsync(latitude, longitude);

            return weeklyWeatherList.ToArray();
        }
    }
}
