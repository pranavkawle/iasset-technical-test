using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services
{
    public class WeatherDataService: IWeatherDataService
    {
        private readonly HttpClient _client;

        public WeatherDataService(IHttpClientAccessor clientAccessor)
        {
            _client = clientAccessor.HttpClient;
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            var httpResponseMessage = await _client.GetAsync(ServiceUrls.Countries);
            httpResponseMessage.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<IEnumerable<Country>>(await StripGeonamesTagFromServiceResponse(httpResponseMessage));
            result = result?.OrderBy(c => c.CountryName);
            return result;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(decimal north, decimal south, decimal east, decimal west)
        {
            var url = string.Format(ServiceUrls.Cities, north, south, east, west);
            var httpResponseMessage = await _client.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await StripGeonamesTagFromServiceResponse(httpResponseMessage);
            var result = JsonConvert.DeserializeObject<IEnumerable<City>>(response);
            result = result?.OrderBy(c => c.Name);
            return result;
        }

        public async Task<WeatherObservation> GetWeatherObservationAsync(decimal latitude, decimal longitude)
        {
            var url = string.Format(ServiceUrls.WeatherObservation, latitude, longitude);
            var httpResponseMessage = await _client.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            response = response.Replace("\"weatherObservation\":", "").Replace("{{", "{").Replace("}}", "}");
            var result = JsonConvert.DeserializeObject<WeatherObservation>(response);
            result.Visibility = string.Format("{0} km", (new Random()).Next(5, 20));
            return result;
        }

        /// <summary>
        /// Strip geonames tag from web service response to return true result.
        /// We can handle this more elegantly
        /// </summary>
        /// <param name="httpResponseMessage">Http response message from web service</param>
        /// <returns></returns>
        private async Task<string> StripGeonamesTagFromServiceResponse(HttpResponseMessage httpResponseMessage)
        {
            var geonamesResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            geonamesResponse = geonamesResponse.Replace("\"geonames\":", "").Trim('{', '}');
            return geonamesResponse;
        }
    }
}