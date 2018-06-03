using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Tests.Services
{
    [TestClass]
    public class WeatherDataServiceTests
    {
        private const string CountriesWebServiceResult = "{\"geonames\":[" +
            "{\"continent\":\"EU\",\"capital\":\"Andorra la Vella\",\"languages\":\"ca\",\"geonameId\":3041565,\"south\":42.42849259876837,\"isoAlpha3\":\"AND\",\"north\":42.65604389629997,\"fipsCode\":\"AN\",\"population\":\"84000\",\"east\":1.7865427778319827,\"isoNumeric\":\"020\",\"areaInSqKm\":\"468.0\",\"countryCode\":\"AD\",\"west\":1.4071867141112762,\"countryName\":\"Andorra\",\"continentName\":\"Europe\",\"currencyCode\":\"EUR\"}," +
            "{\"continent\":\"AS\",\"capital\":\"Abu Dhabi\",\"languages\":\"ar - AE, fa, en, hi, ur\",\"geonameId\":290557,\"south\":22.633329391479492,\"isoAlpha3\":\"ARE\",\"north\":26.08415985107422,\"fipsCode\":\"AE\",\"population\":\"4975593\",\"east\":56.38166046142578,\"isoNumeric\":\"784\",\"areaInSqKm\":\"82880.0\",\"countryCode\":\"AE\",\"west\":51.58332824707031,\"countryName\":\"United Arab Emirates\",\"continentName\":\"Asia\",\"currencyCode\":\"AED\"}," +
            "{\"continent\":\"AS\",\"capital\":\"Kabul\",\"languages\":\"fa - AF, ps, uz - AF, tk\",\"geonameId\":1149361,\"south\":29.377472,\"isoAlpha3\":\"AFG\",\"north\":38.483418,\"fipsCode\":\"AF\",\"population\":\"29121286\",\"east\":74.879448,\"isoNumeric\":\"004\",\"areaInSqKm\":\"647500.0\",\"countryCode\":\"AF\",\"west\":60.478443,\"countryName\":\"Afghanistan\",\"continentName\":\"Asia\",\"currencyCode\":\"AFN\"}]}";
        private const string CitiesWebServiceResult = "{\"geonames\":[" +
            "{\"lng\":149.128074645996,\"geonameId\":2172517,\"countrycode\":\"AU\",\"name\":\"Canberra\",\"fclName\":\"city, village,...\",\"toponymName\":\"Canberra\",\"fcodeName\":\"capital of a political entity\",\"wikipedia\":\"en.wikipedia.org/wiki/Canberra\",\"lat\":-35.2834624726481,\"fcl\":\"P\",\"population\":367752,\"fcode\":\"PPLC\"}," +
            "{\"lng\":151.207323074341,\"geonameId\":2147714,\"countrycode\":\"AU\",\"name\":\"Sydney\",\"fclName\":\"city, village,...\",\"toponymName\":\"Sydney\",\"fcodeName\":\"seat of a first-order administrative division\",\"wikipedia\":\"en.wikipedia.org/wiki/Sydney\",\"lat\":-33.8678499639382,\"fcl\":\"P\",\"population\":4627345,\"fcode\":\"PPLA\"}," +
            "{\"lng\":144.963322877884,\"geonameId\":2158177,\"countrycode\":\"AU\",\"name\":\"Melbourne\",\"fclName\":\"city, village,...\",\"toponymName\":\"Melbourne\",\"fcodeName\":\"seat of a first-order administrative division\",\"wikipedia\":\"en.wikipedia.org/wiki/Melbourne\",\"lat\":-37.8139965641595,\"fcl\":\"P\",\"population\":4246375,\"fcode\":\"PPLA\"}]}";
        private const string WeatherObservationWebServiceResult = "{\"weatherObservation\":" +
            "{\"elevation\":3,\"lng\":151.16666666666666,\"observation\":\"YSSY 271000Z 01006KT CAVOK 18/10 Q1027 NOSIG\",\"ICAO\":\"YSSY\",\"clouds\":\"clouds and visibility OK\",\"dewPoint\":\"10\",\"cloudsCode\":\"CAVOK\",\"datetime\":\"2018-05-27 10:00:00\",\"countryCode\":\"AU\",\"temperature\":\"18\",\"humidity\":59,\"stationName\":\"Sydney Airport\",\"weatherCondition\":\"n/a\",\"windDirection\":10,\"hectoPascAltimeter\":1027,\"windSpeed\":\"06\",\"lat\":-33.95}}";

        private readonly Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly WeatherDataService _service;

        public WeatherDataServiceTests()
        {
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _service = new WeatherDataService(new TestClientAccessor(_mockMessageHandler.Object));
        }

        [TestMethod]
        public async Task GetCountriesAsyncTest_ValidRequest_ReturnsListOfCountriesOrderedByCountryName()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(CountriesWebServiceResult)
                }));

            // Act
            var countries = await _service.GetCountriesAsync();

            // Assert
            Assert.IsNotNull(countries);
            Assert.IsInstanceOfType(countries, typeof(IEnumerable<Country>));
            Assert.AreEqual(3, countries.Count());
            Assert.AreEqual("Afghanistan", countries.First().CountryName);
            Assert.AreEqual("United Arab Emirates", countries.Last().CountryName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), AllowDerivedTypes = false)]
        public async Task GetCountriesAsyncTest_InvalidWebServiceResponse_ReturnsException()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }));

            // Act
            await _service.GetCountriesAsync();

            // Assert: exception expected
        }

        [TestMethod]
        public async Task GetCitiesAsyncTest_ValidRequest_ReturnsListOfCitiesOrderedByCityName()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(CitiesWebServiceResult)
                }));

            const decimal north = -10.062805M,
                south = -43.64397M,
                east = 153.639252M,
                west = 112.911057M;

            // Act
            var cities = await _service.GetCitiesAsync(north, south, east, west);

            // Assert
            Assert.IsNotNull(cities);
            Assert.IsInstanceOfType(cities, typeof(IEnumerable<City>));
            Assert.AreEqual(3, cities.Count());
            Assert.AreEqual("Canberra", cities.First().Name);
            Assert.AreEqual("Sydney", cities.Last().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), AllowDerivedTypes = false)]
        public async Task GetCitiesAsyncTest_InvalidWebServiceResponse_ReturnsException()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }));

            const decimal north = 0,
                south = 0,
                east = 0,
                west = 0;

            // Act
            await _service.GetCitiesAsync(north, south, east, west);

            // Assert: exception expected
        }

        [TestMethod]
        public async Task GetWeatherObservationAsync_ValidRequest_ReturnsListOfCitiesOrderedByCityName()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(WeatherObservationWebServiceResult)
                }));

            const decimal latitude = -33.95M, 
                longitude = 151.16666666666666M;

            // Act
            var observation = await _service.GetWeatherObservationAsync(latitude, longitude);

            // Assert
            Assert.IsNotNull(observation);
            Assert.IsInstanceOfType(observation, typeof(WeatherObservation));
            Assert.AreEqual("clouds and visibility OK", observation.Clouds);
            Assert.AreEqual(new DateTime(2018, 5, 27, 10, 0, 0), observation.Datetime);
            Assert.AreEqual("Sydney Airport", observation.StationName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), AllowDerivedTypes = false)]
        public async Task GetWeatherObservationAsync_InvalidWebServiceResponse_ReturnsException()
        {
            // Arrange
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }));

            const decimal latitude = 0, longitude = 0;

            // Act
            await _service.GetWeatherObservationAsync(latitude, longitude);

            // Assert: exception expected
        }
    }
}