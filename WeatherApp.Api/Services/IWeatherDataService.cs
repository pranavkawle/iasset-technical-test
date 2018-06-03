using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services
{
    public interface IWeatherDataService
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(decimal north, decimal south, decimal east, decimal west);
        Task<WeatherObservation> GetWeatherObservationAsync(decimal lat, decimal lng);
    }
}
