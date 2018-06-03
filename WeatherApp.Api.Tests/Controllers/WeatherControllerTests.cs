using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WeatherApp.Api.Controllers;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Tests.Controllers
{
    [TestClass]
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherDataService> _mockWeatherDataService;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _mockWeatherDataService = new Mock<IWeatherDataService>();
            _controller = new WeatherController(_mockWeatherDataService.Object);
        }

        [TestMethod]
        public void GetCountries_ValidRequest_ReturnsListOfCountries()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetCountriesAsync())
                .ReturnsAsync(new List<Country>
                {
                    new Country { CountryName = "Afghanistan", CountryCode = "AF", ContinentName = "Asia" },
                    new Country { CountryName = "Andorra", CountryCode = "AD", ContinentName = "Europe" },
                    new Country { CountryName = "United Arab Emirates", CountryCode = "AE", ContinentName = "Asia" }
                });

            // Act
            var response = _controller.GetCountries();
            var httpResult = response.GetAwaiter().GetResult() as OkNegotiatedContentResult<IEnumerable<Country>>;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(OkNegotiatedContentResult<IEnumerable<Country>>));
            Assert.IsNotNull(httpResult.Content);
            var countries = httpResult.Content;
            Assert.AreEqual(3, countries.Count());
            Assert.AreEqual("Afghanistan", countries.First().CountryName);
            Assert.AreEqual("United Arab Emirates", countries.Last().CountryName);
        }

        [TestMethod]
        public void GetCountries_ServiceReturnException_ReturnsErrorMessage()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetCountriesAsync())
                .ThrowsAsync(new HttpRequestException());

            // Act
            var response = _controller.GetCountries();
            var httpResult = response.GetAwaiter().GetResult() as ExceptionResult;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(ExceptionResult));
        }

        [TestMethod]
        public void GetCities_ValidRequest_ReturnsListOfCities()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetCitiesAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(new List<City>
                {
                    new City { CountryCode = "AU", Name = "Canberra" },
                    new City { CountryCode = "AU", Name = "Melbourne" },
                    new City { CountryCode = "AU", Name = "Sydney" }
                });
            const decimal north = -10.062805M,
                south = -43.64397M,
                east = 153.639252M,
                west = 112.911057M;

            // Act
            var response = _controller.GetCities(north, south, east, west);
            var httpResult = response.GetAwaiter().GetResult() as OkNegotiatedContentResult<IEnumerable<City>>;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(OkNegotiatedContentResult<IEnumerable<City>>));
            Assert.IsNotNull(httpResult.Content);
            var cities = httpResult.Content;
            Assert.AreEqual(3, cities.Count());
            Assert.AreEqual("Canberra", cities.First().Name);
            Assert.AreEqual("Sydney", cities.Last().Name);
        }

        [TestMethod]
        public void GetCities_ServiceReturnException_ReturnsErrorMessage()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetCitiesAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ThrowsAsync(new HttpRequestException());
            const decimal north = -10.062805M,
                south = -43.64397M,
                east = 153.639252M,
                west = 112.911057M;

            // Act
            var response = _controller.GetCities(north, south, east, west);
            var httpResult = response.GetAwaiter().GetResult() as ExceptionResult;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(ExceptionResult));
        }

        [TestMethod]
        public void GetObservation_ValidRequest_ReturnsWeatherObservation()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetWeatherObservationAsync(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(new WeatherObservation
                {
                    Observation = "YSSY 271000Z 01006KT CAVOK 18/10 Q1027 NOSIG",
                    Icao = "YSSY",
                    Clouds = "clouds and visibility OK",
                    DewPoint = 10,
                    CloudsCode = "CAVOK",
                    Datetime = new DateTime(2018, 5, 27, 10, 0, 0),
                    CountryCode = "AU",
                    Temperature = 18,
                    Humidity = 59,
                    StationName = "Sydney Airport"
                });
            const decimal latitude = -33.95M,
                longitude = 151.16666666666666M;

            // Act
            var response = _controller.GetObservation(latitude, longitude);
            var httpResult = response.GetAwaiter().GetResult() as OkNegotiatedContentResult<WeatherObservation>;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(OkNegotiatedContentResult<WeatherObservation>));
            Assert.IsNotNull(httpResult.Content);
            Assert.AreEqual("AU", httpResult.Content.CountryCode);
            Assert.AreEqual(18, httpResult.Content.Temperature);
            Assert.AreEqual(59, httpResult.Content.Humidity);
        }

        [TestMethod]
        public void GetObservation_ServiceReturnException_ReturnsErrorMessage()
        {
            // Arrange
            _mockWeatherDataService
                .Setup(w => w.GetWeatherObservationAsync(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ThrowsAsync(new HttpRequestException());
            const decimal latitude = -33.95M,
                longitude = 151.16666666666666M;

            // Act
            var response = _controller.GetObservation(latitude, longitude);
            var httpResult = response.GetAwaiter().GetResult() as ExceptionResult;

            // Assert
            Assert.IsNotNull(httpResult);
            Assert.IsInstanceOfType(httpResult, typeof(ExceptionResult));
        }
    }
}