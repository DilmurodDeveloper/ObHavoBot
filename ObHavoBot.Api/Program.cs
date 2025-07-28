using ObHavoBot.Api.Brokers.Telegrams;
using ObHavoBot.Api.Brokers.Weathers;
using ObHavoBot.Api.Services.Foundations.Weathers;
using ObHavoBot.Api.Services.Orchestrations.WeatherOrchestrations;
using ObHavoBot.Api.Services.Processings.TelegramMessages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITelegramBotBroker, TelegramBotBroker>();
builder.Services.AddScoped<IWeatherBroker, WeatherBroker>();
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<IWeatherOrchestrationService, WeatherOrchestrationService>();
builder.Services.AddTransient<ITelegramMessageProcessingService, TelegramMessageProcessingService>();

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