using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadyTeachApi.Models;
using ReadyTeachApi.Services.Interfaces;

namespace ReadyTeachApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeService _coffeeService;

        public CoffeeController(ICoffeeService coffeeService) 
        {
            _coffeeService = coffeeService;
        }
        [HttpGet]
        public IActionResult getBrewCoffee()
        {
            var coffeeModel = _coffeeService.BrewCoffee();

            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                return StatusCode(418, "I'm a teapot");
            }

            if (coffeeModel == null)
            {
                return StatusCode(503, "Service Unavailable - Coffee not available");
            }

            return Ok(coffeeModel);
        }
    }
}
