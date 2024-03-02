
using Microsoft.AspNetCore.Http;
using ReadyTeachApi.Models;
using ReadyTeachApi.Services.Interfaces;
using ReadyTechApi.DataAccess.Entities;
using System.Text.Json;

namespace ReadyTeachApi.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly IServiceDate _serviceDate;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public CoffeeService(ISessionHelper sessionHelper, IServiceDate serviceDate, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _sessionHelper = sessionHelper;
            _serviceDate = serviceDate;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CoffeeModel> BrewCoffee()
        {
            DateTime today = _serviceDate.GetDate();

            var temperature = await getWeatherInformation();

            var responseMessage = temperature > 30
                                    ? "Your refreshing iced coffee is ready"
                                    : "Your piping hot coffee is ready";

            if (today.Month == 4 && today.Day == 1)
            {
                return null;
            }

            int? sessionCount = await _sessionHelper.Get("BreCoffeeSessionCount") ?? 0;
            sessionCount++;
            await _sessionHelper.Add("BreCoffeeSessionCount", sessionCount);

            if (sessionCount % 5 == 0)
            {
                return null;
            }

            return new CoffeeModel
            {
                Message = $"{responseMessage}",
                Prepared = $"{today}"
            };
        }

        private async Task<double> getWeatherInformation()
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var weatherURL = _config.GetValue<string>("WeatherAPI:API-url");
            var AppId = _config.GetValue<string>("WeatherAPI:APPID");
            var location = "London,uk";

            var requestURL = $"{weatherURL}?q={location}&APPID={AppId}";

            var httpResponseMessage = await httpClient.GetAsync(requestURL);

            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            var weatherInfo = await JsonSerializer.DeserializeAsync
                <WeatherInformation>(contentStream);

            var tempInCelcius = weatherInfo.main.temp - 273.15;

            return tempInCelcius;
        }
    }
}