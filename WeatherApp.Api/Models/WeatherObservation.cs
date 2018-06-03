using System;

namespace WeatherApp.Api.Models
{
    public class WeatherObservation
    {
        public string Observation { get; set; }
        public DateTime Datetime { get; set; }
        public string StationName { get; set; }
        public string Icao { get; set; }
        public string CountryCode { get; set; }
        public decimal Elevation { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public decimal Temperature { get; set; }
        public decimal DewPoint { get; set; }
        public short Humidity { get; set; }
        public string Clouds { get; set; }
        public string CloudsCode { get; set; }
        public string WeatherCondition { get; set; }
        public decimal HectoPascAltimeter { get; set; }
        public short WindDirection { get; set; }
        public short WindSpeed { get; set; }
        public string Visibility { get; set; }
    }
}