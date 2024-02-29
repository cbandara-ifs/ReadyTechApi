
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

        public CoffeeModel BrewCoffee()
        {
            DateTime today = _serviceDate.GetDate();

            if (today.Month == 4 && today.Day == 1)
            {
                return null;
            }

            int sessionCount = _sessionHelper.Get("BreCoffeeSessionCount") ?? 0;
            sessionCount++;
            _sessionHelper.Add("BreCoffeeSessionCount", sessionCount);

            if (sessionCount % 5 == 0) 
            {
                return null;
            }

            return new CoffeeModel
            {
                Message = "Your piping hot coffee is ready",
                Prepared = "2021-02-03T11:56:24+0900"
            };
        }
    }
}