using AutoFixture;
using FlightsAPI.Repositories;
using FlightsAPI.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlightsAPI.Domain.Models;
using Xunit;

namespace FlightsAPITests.Services
{
    public class LuggageServiceTests
    {
        private readonly LuggageService _sut;
        private readonly Mock<ILuggageRepository> _luggageRepository;
        private readonly Fixture _fixture = new();

        public LuggageServiceTests()
        {
            _luggageRepository = new();
            _sut = new(_luggageRepository.Object);
        }

        [Fact]
        public void GetAllReturnsResult()
        {
            // Arrange
            var list = _fixture.CreateMany<Luggage>(3).ToList();
            _luggageRepository.Setup(x => x.GetAll()).Returns(list);

            // Act
            var result = _sut.GetLuggage();

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
        public void GetByIdThrowsWhenIdDoesNotExist()
        {
            // Arrange
            var list = _fixture.CreateMany<Luggage>(3).ToList();
            list[0].Id = 5;

            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = () => _sut.GetLuggage(5);

            // Assert
            result.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task AddLuggageThrowsOnInvalidId()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            luggage.PassengerId = 0;

            // Act
            var result = async () => await _sut.AddLuggage(luggage);

            // Assert
            await result.Should().ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task AddLuggageReturnsOkWhenParametersAreValid()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();

            // Act
            await _sut.AddLuggage(luggage);

            // Assert
            _luggageRepository.Verify(x => x.AddAsync(It.IsAny<Luggage>()), Times.Once);
        }

        [Fact]
        public void UpdateLuggageExecutesWithValidParameters()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(luggage);

            // Act
            _sut.UpdateLuggage(luggage);

            // Assert
            _luggageRepository.Verify(x => x.Update(It.IsAny<Luggage>()), Times.Once);
        }

        [Fact]
        public void UpdateLuggageThrowsWithInValidParameters()
        {
            // Arrange
            var newLuggage = _fixture.Create<Luggage>();
            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((Luggage) null);

            // Act
            var result = () => _sut.UpdateLuggage(newLuggage);

            // Assert
            result.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public void DeleteLuggageExecutesWithValidParameters()
        {
            // Arrange
            var luggage = _fixture.Create<Luggage>();
            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(luggage);

            // Act
            _sut.DeleteLuggage(luggage.Id);

            // Assert
            _luggageRepository.Verify(x => x.Delete(It.IsAny<Luggage>()), Times.Once);
        }

        [Fact]
        public void DeleteLuggageThrowsWithInValidParameters()
        {
            // Arrange
            _luggageRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((Luggage) null);

            // Act
            var result = () => _sut.DeleteLuggage(It.IsAny<int>());

            // Assert
            result.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetMostPopularReturnsCorrectResult()
        {
            // Arrange
            var luggages = _fixture.CreateMany<Luggage>().ToList();
            luggages[0].LuggageTypeId = 1;
            luggages[1].LuggageTypeId = 1;
            var luggageTypes = _fixture.CreateMany<LuggageType>().ToList();
            luggageTypes[0].Id = 1;
            luggageTypes[1].Id = 1;

            _luggageRepository.Setup(x => x.GetAll()).Returns(luggages);
            _luggageRepository.Setup(x => x.GetLuggageTypes()).Returns(luggageTypes);

            var expected = new Luggage()
            {
                Id = It.IsAny<int>(),
                LuggageTypeId = 1,
                PassengerId = It.IsAny<int>()
            };

            // Act
            var result = _sut.GetMostPopularLuggage();

            // Assert
            result.Id.Should().Be(expected.LuggageTypeId);
        }

        [Fact]
        public void GetMostPopularThrowsWhenLuggageTypesIsEmpty()
        {
            _luggageRepository.Setup(x => x.GetAll()).Returns(new List<Luggage>());

            var result = () => _sut.GetMostPopularLuggage();

            result.Should().Throw<ArgumentException>();
        }
    }
}
