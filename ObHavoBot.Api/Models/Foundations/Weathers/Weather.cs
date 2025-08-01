﻿namespace ObHavoBot.Api.Models.Foundations.Weathers
{
    public class Weather
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public string Description { get; set; }
    }
}
