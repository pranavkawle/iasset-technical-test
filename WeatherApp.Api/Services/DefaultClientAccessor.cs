using System.Net.Http;

namespace WeatherApp.Api.Services
{
    public class DefaultClientAccessor : IHttpClientAccessor
    {
        public HttpClient HttpClient => new HttpClient();
    }
}