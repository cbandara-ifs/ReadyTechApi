
using Microsoft.AspNetCore.Http;
using ReadyTeachApi.Models;
using ReadyTeachApi.Services.Interfaces;

namespace ReadyTeachApi.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly ISessionHelper _sessionHelper;
        private readonly IServiceDate _serviceDate;

        public CoffeeService(ISessionHelper sessionHelper, IServiceDate serviceDate)
        {
            _sessionHelper = sessionHelper;
            _serviceDate = serviceDate;
        }

        public async Task<CoffeeModel> BrewCoffee()
        {
            DateTime today = _serviceDate.GetDate();

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
                Message = "Your piping hot coffee is ready",
                Prepared = $"{today}"
            };
        }
    }
}