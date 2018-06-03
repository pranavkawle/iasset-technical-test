using System;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Controllers
{
    public class WeatherController : ApiController
    {
        private readonly IWeatherDataService _weatherDataService;

        public WeatherController(IWeatherDataService weatherDataService)
        {
            _weatherDataService = weatherDataService;
        }

        [Route("api/get-countries")]
        public async Task<IHttpActionResult> GetCountries()
        {
            try
            {
                var countries = await _weatherDataService.GetCountriesAsync();
                return Ok(countries);
            }
            catch
            {
                return InternalServerError(new Exception("Error occurred while fetching countries."));
            }
        }

        [Route("api/get-cities")]
        public async Task<IHttpActionResult> GetCities(decimal north, decimal south, decimal east, decimal west)
        {
            try
            {
                var cities = await _weatherDataService.GetCitiesAsync(north, south, east, west);
                return Ok(cities);
            }
            catch
            {
                return InternalServerError(new Exception("Error occurred while fetching cities."));
            }
        }

        [Route("api/get-observation")]
        public async Task<IHttpActionResult> GetObservation(decimal latitude, decimal longitude)
        {
            try
            {
                var observation = await _weatherDataService.GetWeatherObservationAsync(latitude, longitude);
                return Ok(observation);
            }
            catch
            {
                return InternalServerError(new Exception("No observation found."));
            }
        }
    }
}
