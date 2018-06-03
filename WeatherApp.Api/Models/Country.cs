namespace WeatherApp.Api.Models
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string IsoNumeric { get; set; }
        public string IsoAlpha3 { get; set; }
        public string FipsCode { get; set; }
        public string Continent { get; set; }
        public string ContinentName { get; set; }
        public string Capital { get; set; }
        public double AreaInSqKm { get; set; }
        public long Population { get; set; }
        public string CurrencyCode { get; set; }
        public string Languages { get; set; }
        public int GeonameId { get; set; }
        public decimal West { get; set; }
        public decimal North { get; set; }
        public decimal East { get; set; }
        public decimal South { get; set; }
        public string PostalCodeFormal { get; set; }
    }
}