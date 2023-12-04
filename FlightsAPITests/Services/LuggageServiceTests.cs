using AutoFixture;
using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlightsAPITests.Services
{
    public class LuggageServiceTests
    {
        private readonly LuggageService _sut;
        private readonly Mock<ILuggageRepository> _luggageRepository;
        private readonly Mock<ILogger> _logger;
        private readonly Fixture _fixture = new();

        public LuggageServiceTests()
        {
            _logger = new Mock<ILogger>();
            _luggageRepository = new();
            _sut = new(_luggageRepository.Object, _logger.Object);
        }

        [Fact]
        public void GetAllReturnsResult()
        {
            // Arrange
            var list = _fixture.CreateMany<Luggage>(3).ToList();
            _luggageRepository.Setup(x => x.GetAll()).Returns(list);

            // Act
            var result = _sut.GetLuggages();

            // Assert
            result.Should().BeEquivalentTo(list);
        }

        [Fact]
        public void GetByIdReturnsResult()
        {
            // Arrange
            var list = _fixture.CreateMany<Luggage>(3).ToList();
            list[0].Id = 5;

            _luggageRepository.Setup(x => x.GetById(5)).Returns(list[0]);

            // Act
            var result = _sut.GetLuggage(5);

            // Assert
            result.Should().BeEquivalentTo(list[0]);
        }

        [Fact]
        public async Task AddLuggageReturnsBadRequestOnInvalidId()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            luggage.PassengerId = 0;

            // Act
            var result = await _sut.AddLuggage(luggage.PassengerId, luggage.LuggageTypeId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddLuggageReturnsOkWhenParametersAreValid()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();

            // Act
            var result = await _sut.AddLuggage(luggage.PassengerId, luggage.LuggageTypeId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void UpdateLuggageReturnsOkResultWithValidParameters()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(luggage);

            // Act
            var result = _sut.UpdateLuggage(luggage.Id, luggage.LuggageTypeId, luggage.PassengerId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void UpdateLuggageReturnsBadRequestResultWithInValidParameters()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            _luggageRepository.Setup(x => x.GetById(luggage.Id)).Throws<InvalidOperationException>();

            // Act
            var result = _sut.UpdateLuggage(luggage.Id, luggage.LuggageTypeId, luggage.PassengerId);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
            _logger.Verify();
        }
    }
}
