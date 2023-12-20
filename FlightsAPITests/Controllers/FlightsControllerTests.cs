using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Controllers;
using FlightsAPI.Domain.Models;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightsAPITests.Controllers
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFlightsService> _service;
        private readonly FlightsController _sut;
        private readonly Fixture _fixture;

        public FlightsControllerTests()
        {
            _service = new Mock<IFlightsService>();
            _sut = new FlightsController(_service.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetFlightsReturnsOk()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            _service.Setup(x => x.GetFlights()).Returns(flights);

            // Act
            var result = _sut.GetFlights();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetFlightsReturnsNoContentWhenListIsEmpty()
        {
            // Arrange
            _service.Setup(x => x.GetFlights()).Returns(new List<Flight>());

            // Act
            var result = _sut.GetFlights();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void GetFlightReturnsOk()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            _service.Setup(x => x.GetFlight(flight.Id)).Returns(flight);

            // Act
            var result = _sut.GetFlight(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetFlightReturnsNotFoundWhenFlightDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.GetFlight(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.GetFlight(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Theory]
        [InlineData("origin")]
        [InlineData("destination")]
        public void TopFiveFlightsReturnsOk(string direction)
        {
            // Arrange
            var flights = _fixture.CreateMany<TopFiveDto>().ToList();
            _service.Setup(x => x.TopFiveFlights(direction)).Returns(flights);

            // Act
            var result = _sut.GetTopFiveFlights(direction);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void TopFiveReturnsNotFoundWhenDirectionIsIncorrect()
        {
            // Arrange
            _service.Setup(x => x.TopFiveFlights("bla")).Throws<ArgumentException>();

            // Act
            var result = _sut.GetTopFiveFlights("bla");

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddFlightReturnsCreatedAtRoute()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();

            // Act
            var result = await _sut.AddFlight(
                flight.Origin,
                flight.Destination, 
                flight.DepartureTime, 
                flight.ArrivalTime, 
                flight.PlaneId);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task AddReturnsNotFoundWhenFlightDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.AddFlight(It.IsAny<Flight>())).Throws<InvalidOperationException>();

            // Act
            var result = await _sut.AddFlight(
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<DateTime>(), 
                It.IsAny<int>());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void PatchFlightReturnsOk()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();

            // Act
            var result = _sut.PatchFlight(
                flight.Id,
                flight.Origin,
                flight.Destination,
                flight.DepartureTime,
                flight.ArrivalTime,
                flight.PlaneId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void PatchReturnsNotFoundWhenFlightDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.EditFlight(It.IsAny<Flight>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.PatchFlight(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void DeleteFlightReturnsNoContent()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();

            // Act
            var result = _sut.DeleteFlight(flight.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteReturnsNotFoundWhenFlightDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.DeleteFlight(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.DeleteFlight(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
