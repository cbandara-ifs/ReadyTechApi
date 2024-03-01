using Microsoft.AspNetCore.Http;
using Moq;
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

        public CoffeeTests() 
        {
            _mockSessionHelper = new Mock<ISessionHelper>();
            _mockServiceDate = new Mock<IServiceDate>();
        }
        [Fact]
        public async Task ShouldReturnStatusMessage()
        {
            DateTime today = DateTime.Now;

            Coffee response = new Coffee
            {
                Message = "Your piping hot coffee is ready",
                Prepared = $"{today}"
            };

            _mockSessionHelper.Setup(s => s.Get("BreCoffeeSessionCount")).ReturnsAsync(1);
            _mockServiceDate.Setup(s => s.GetDate()).Returns(today);

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object);

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

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object);

            CoffeeModel result = await processor.BrewCoffee();

            Assert.Null(result);
        }

        [Fact]
        public async Task shouldReturnEmptyOnAprilFirst()
        {
            DateTime today = new DateTime(2024, 04, 01);

            _mockSessionHelper.Setup(s => s.Get("BreCoffeeSessionCount")).ReturnsAsync(1);
            _mockServiceDate.Setup(s => s.GetDate()).Returns(today);

            CoffeeService processor = new CoffeeService(_mockSessionHelper.Object, _mockServiceDate.Object);

            CoffeeModel result = await processor.BrewCoffee();

            Assert.Null(result);
        }
    }
}
