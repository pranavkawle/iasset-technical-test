using System.Net.Http;

namespace WeatherApp.Api.Services
{
    public interface IHttpClientAccessor
    {
        HttpClient HttpClient { get; }
    }
}
