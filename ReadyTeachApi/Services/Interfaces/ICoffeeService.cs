using ReadyTeachApi.Models;

namespace ReadyTeachApi.Services.Interfaces
{
    public interface ICoffeeService
    {
        public Task<CoffeeModel> BrewCoffee();
    }
}
