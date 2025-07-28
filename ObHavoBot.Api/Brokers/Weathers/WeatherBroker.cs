using System.Text.Json;
using ObHavoBot.Api.Models.Foundations.Weathers;

namespace ObHavoBot.Api.Brokers.Weathers
{
    public class WeatherBroker : IWeatherBroker
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;

        public WeatherBroker(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.apiKey = configuration["OpenWeatherMap:ApiKey"];
        }

        public async ValueTask<Weather> GetTodayWeatherByCoordinatesAsync(double lat, double lon)
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

            string response = await httpClient.GetStringAsync(url);
            using JsonDocument document = JsonDocument.Parse(response);
            JsonElement root = document.RootElement;

            return new Weather
            {
                City = root.GetProperty("name").GetString(),
                Date = DateTime.UtcNow,
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString()
            };
        }

        public async ValueTask<List<Weather>> Get7DayForecastByCoordinatesAsync(double lat, double lon)
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=current,minutely,hourly,alerts&appid={apiKey}&units=metric";

            string response = await httpClient.GetStringAsync(url);
            using JsonDocument document = JsonDocument.Parse(response);
            JsonElement dailyArray = document.RootElement.GetProperty("daily");

            var weatherList = new List<Weather>();

            foreach (JsonElement day in dailyArray.EnumerateArray().Take(7))
            {
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(day.GetProperty("dt").GetInt64()).DateTime;
                double temperature = day.GetProperty("temp").GetProperty("day").GetDouble();
                string description = day.GetProperty("weather")[0].GetProperty("description").GetString();

                weatherList.Add(new Weather
                {
                    Date = date,
                    Temperature = temperature,
                    Description = description
                });
            }

            return weatherList;
        }
    }
}
