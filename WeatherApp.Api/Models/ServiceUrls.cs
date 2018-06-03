namespace WeatherApp.Api.Models
{
    public static class ServiceUrls
    {
        /// <summary>
        /// Url to get list of countries
        /// </summary>
        public static string Countries => "http://api.geonames.org/countryInfoJSON?username=pkawle";

        /// <summary>
        /// Url to get list of cities using coordinates in order => North, South, East, West
        /// </summary>
        public static string Cities => "http://api.geonames.org/citiesJSON?north={0}&south={1}&east={2}&west={3}&username=pkawle";

        /// <summary>
        /// Url to get weather observation near provided latitude and longitude
        /// </summary>
        public static string WeatherObservation => "http://api.geonames.org/findNearByWeatherJSON?lat={0}&lng={1}&username=pkawle";
    }
}