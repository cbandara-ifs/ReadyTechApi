using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using ReadyTeachApi.DataAccess.Entities;
using ReadyTeachApi.Models;
using ReadyTeachApi.Services;
using ReadyTeachApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTeachApi.Test
{
    public class CoffeeTests
    {
        private readonly Mock<ISessionHelper> _mockSessionHelper;
        private readonly Mock<IServiceDate> _mockServiceDate;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly IConfiguration _configuration;

        public CoffeeTests()
        {
            _mockSessionHelper = new Mock<ISessionHelper>();
            _mockServiceDate = new Mock<IServiceDate>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var inMemorySettings = new Dictionary<string, string?> {
                {"WeatherAPI:API-url", "https://api.openweathermap.org/data/2.5/weather"},
                {"WeatherAPI:APPID", "9872eaea3dfab429febb6fd44106292f"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Theory]
        [InlineData(25, "Your piping hot coffee is ready")] // Temperature <= 30
        [InlineData(35, "Your refreshing iced coffee is ready")] // Temperature > 30
        public async Task ShouldReturnStatusMessage(int temperature, string expectedMessage)
        {
            DateTime today = DateTime.Now;

            var weatherJSON = "";
            if (temperature == 25)
            {
                weatherJSON = "{\"main\":{\"temp\":298.15}}";
            }
            else
            {
                weatherJSON = "{\"main\":{\"temp\":308.15}}";
            }

            Coffee response = new Coffee
            {
                Message = $"{expectedMessage}",
                Prepared = $"{today}"
            };

            _mockSessionHelper.Setup(s => s.Get("BreCoffeeSessionCount")).ReturnsAsync(1);
            _mockServiceDate.Setup(s => s.GetDate()).Returns(today);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(weatherJSON)
            });

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object, _configuration, _mockHttpClientFactory.Object);

            CoffeeModel result = await processor.BrewCoffee();

            Assert.NotNull(result);

            Assert.Equal(response.Message, result.Message);
            Assert.Equal(response.Prepared, result.Prepared);
        }

        [Fact]
        public async Task ShouldReturnServiceUnavailable()
        {
            _mockSessionHelper.Setup(s => s.Get("BreCoffeeSessionCount")).ReturnsAsync(4);
            _mockServiceDate.Setup(s => s.GetDate()).Returns(DateTime.Now);

            var httpClient = new HttpClient();

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object, _configuration, _mockHttpClientFactory.Object);

            CoffeeModel result = await processor.BrewCoffee();

            Assert.Null(result);
        }

        [Fact]
        public async Task shouldReturnEmptyOnAprilFirst()
        {
            DateTime today = new DateTime(2024, 04, 01);

            _mockSessionHelper.Setup(s => s.Get("BreCoffeeSessionCount")).ReturnsAsync(1);
            _mockServiceDate.Setup(s => s.GetDate()).Returns(today);

            var httpClient = new HttpClient();

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object, _configuration, _mockHttpClientFactory.Object);

            CoffeeModel result = await processor.BrewCoffee();

            Assert.Null(result);
        }
    }
}
