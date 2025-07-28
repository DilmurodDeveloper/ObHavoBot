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

        public async ValueTask<List<Weather>> Get5Day3HourForecastByCoordinatesAsync(double lat, double lon)
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

            string response = await httpClient.GetStringAsync(url);
            using JsonDocument document = JsonDocument.Parse(response);
            JsonElement root = document.RootElement;

            var listElement = root.GetProperty("list");

            var groupedByDate = new Dictionary<DateTime, List<(double TempMin, double TempMax, string Description)>>();

            foreach (JsonElement item in listElement.EnumerateArray())
            {
                long dt = item.GetProperty("dt").GetInt64();
                DateTime dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(dt).UtcDateTime;
                DateTime date = dateTimeUtc.Date;

                double tempMin = item.GetProperty("main").GetProperty("temp_min").GetDouble();
                double tempMax = item.GetProperty("main").GetProperty("temp_max").GetDouble();
                string description = item.GetProperty("weather")[0].GetProperty("description").GetString();

                if (!groupedByDate.ContainsKey(date))
                    groupedByDate[date] = new List<(double, double, string)>();

                groupedByDate[date].Add((tempMin, tempMax, description));
            }

            var forecasts = new List<Weather>();

            foreach (var kvp in groupedByDate)
            {
                DateTime date = kvp.Key;
                var tempsAndDescriptions = kvp.Value;

                double minTemp = tempsAndDescriptions.Min(x => x.TempMin);
                double maxTemp = tempsAndDescriptions.Max(x => x.TempMax);

                string mostCommonDescription = tempsAndDescriptions
                    .GroupBy(x => x.Description)
                    .OrderByDescending(g => g.Count())
                    .First().Key;

                forecasts.Add(new Weather
                {
                    Date = date,
                    TempMin = minTemp,
                    TempMax = maxTemp,
                    Description = mostCommonDescription
                });
            }

            return forecasts.OrderBy(w => w.Date).Take(5).ToList();
        }
    }
}
