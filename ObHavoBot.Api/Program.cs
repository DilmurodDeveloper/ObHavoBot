using ObHavoBot.Api.Brokers.Telegrams;
using ObHavoBot.Api.Brokers.Weathers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITelegramBotBroker, TelegramBotBroker>();
builder.Services.AddScoped<IWeatherBroker, WeatherBroker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
