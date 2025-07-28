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
            List<Weather> forecast3hList =
                await this.weatherBroker.Get5Day3HourForecastByCoordinatesAsync(latitude, longitude);

            var groupedByDay = forecast3hList
                .GroupBy(f => f.Date.Date)
                .Select(g => new Weather
                {
                    Date = g.Key,
                    TempMin = g.Min(x => x.TempMin),
                    TempMax = g.Max(x => x.TempMax),
                    Description = g
                        .GroupBy(x => x.Description)
                        .OrderByDescending(x => x.Count())
                        .First().Key
                })
                .Take(5)
                .ToArray();

            return groupedByDay;
        }
    }
}
