using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Tests
{
    public class TestClientAccessor : IHttpClientAccessor
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public TestClientAccessor(HttpMessageHandler httpMessageHandler)
        {
            this._httpMessageHandler = httpMessageHandler;
        }

        public HttpClient HttpClient => new HttpClient(_httpMessageHandler);
    }
}
