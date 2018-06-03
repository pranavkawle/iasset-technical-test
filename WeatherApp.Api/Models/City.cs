namespace WeatherApp.Api.Models
{
    public class City
    {
        public string ToponymName { get; set; }
        public string Name { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public int GeonameId { get; set; }
        public string CountryCode { get; set; }
        public string Fcl { get; set; }
        public string Fcode { get; set; }
    }
}