using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Controllers;
using FlightsAPI.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightsAPITests.Controllers
{
    public class LuggageControllerTests
    {
        private readonly Mock<ILuggageService> _service;
        private readonly LuggageController _sut;
        private readonly Fixture _fixture;

        public LuggageControllerTests()
        {
            _service = new Mock<ILuggageService>();
            _sut = new LuggageController(_service.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetLuggageReturnsOk()
        {
            // Arrange
            var luggage = _fixture.CreateMany<Luggage>().ToList();
            _service.Setup(x => x.GetLuggage()).Returns(luggage);

            // Act
            var result = _sut.GetLuggage();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetLuggageReturnsNoContentWhenListIsEmpty()
        {
            // Arrange
            _service.Setup(x => x.GetLuggage()).Returns(new List<Luggage>());

            // Act
            var result = _sut.GetLuggage();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void GetLuggageByIdReturnsOk()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            _service.Setup(x => x.GetLuggage(luggage.Id)).Returns(luggage);

            // Act
            var result = _sut.GetLuggage(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetLuggageByIdReturnsNotFoundWhenLuggageDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.GetLuggage(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.GetLuggage(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddLuggageReturnsCreatedAtRoute()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();

            // Act
            var result = await _sut.AddLuggage(luggage.LuggageTypeId, luggage.PassengerId);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task AddReturnsNotFoundWhenLuggageDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.AddLuggage(It.IsAny<Luggage>())).Throws<InvalidOperationException>();

            // Act
            var result = await _sut.AddLuggage(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void EditLuggageReturnsOk()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();

            // Act
            var result = _sut.EditLuggage(luggage.Id, luggage.LuggageTypeId, luggage.PassengerId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void EditReturnsNotFoundWhenLuggageDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.UpdateLuggage(It.IsAny<Luggage>())).Throws<InvalidDataException>();

            // Act
            var result = _sut.EditLuggage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void DeleteLuggageReturnsNoContent()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();

            // Act
            var result = _sut.DeleteLuggage(luggage.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteReturnsNotFoundWhenLuggageDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.DeleteLuggage(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.DeleteLuggage(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void GetPopularReturnsOk()
        {
            // Arrange
            var luggageType = _fixture.Create<LuggageType>();
            _service.Setup(x => x.GetMostPopularLuggage()).Returns(luggageType);

            // Act
            var result = _sut.GetPopularLuggage();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetPopularReturnsNotFoundWhenLuggageTypeDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.GetMostPopularLuggage()).Throws<ArgumentException>();

            // Act
            var result = _sut.GetPopularLuggage();

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

    }
}
