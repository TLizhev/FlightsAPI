using FlightsAPI.Controllers;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightsAPITests.Controllers
{
    public class DiscountsControllerTests
    {
        private readonly DiscountsController _sut;
        private readonly Mock<IDiscountsService> _discountsService;

        public DiscountsControllerTests()
        {
            _discountsService = new Mock<IDiscountsService>();
            _sut = new DiscountsController(_discountsService.Object);
        }

        [Fact]
        public void GetDiscountReturnsOk()
        {
            // Arrange
            _discountsService.Setup(x => x.CalculateDiscount(It.IsAny<int>())).Returns(5);
            // Act
            var result = _sut.GetDiscount(It.IsAny<int>());
            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
