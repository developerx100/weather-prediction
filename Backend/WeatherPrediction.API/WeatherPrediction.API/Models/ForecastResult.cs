using System;
using System.Collections.Generic;

namespace WeatherPrediction.API.Models
{
    public class ForecastResult
    {
        public string City { get; set; }
        public List<ForecastDetails> Forecast { get; set; }
    }

    public class ForecastDetails
    {
        public DateTime Date { get; set; }
        public double HighTemp { get; set; }
        public double LowTemp { get; set; }
        public string Advice { get; set; }
    }
}
