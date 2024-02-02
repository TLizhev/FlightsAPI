using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Domain.Models;
using FlightsAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightsAPITests.Services;

public class CabinCrewServiceTests
{
    private readonly CabinCrewService _sut;
    private readonly Mock<ICabinCrewRepository> _cabinCrewRepository;
    private readonly Fixture _fixture = new();

    public CabinCrewServiceTests()
    {
        _cabinCrewRepository = new();
        _sut = new(_cabinCrewRepository.Object);
    }

    [Fact]
    public void GetAllReturnsResult()
    {
        // Arrange
        var list = _fixture.CreateMany<CabinCrew>(3).ToList();
        _cabinCrewRepository.Setup(x => x.GetAll()).Returns(list);

        // Act
        var result = _sut.GetCabinCrew();

        // Assert
        result.Should().BeEquivalentTo(list);
    }

    [Fact]
    public void GetByIdReturnsResult()
    {
        // Arrange
        var list = _fixture.CreateMany<CabinCrew>(3).ToList();
        list[0].Id = 5;

        _cabinCrewRepository.Setup(x => x.GetById(5)).Returns(list[0]);

        // Act
        var result = _sut.GetCabinCrew(5);

        // Assert
        result.Should().BeEquivalentTo(list[0]);
    }

    [Fact]
    public void GetByIdThrowsWhenIdDoesNotExist()
    {
        // Arrange
        var list = _fixture.CreateMany<CabinCrew>(3).ToList();
        list[0].Id = 5;

        _cabinCrewRepository.Setup(x => x.GetById(It.IsAny<int>())).Throws<InvalidOperationException>();

        // Act
        var result = () => _sut.GetCabinCrew(5);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task AddLuggageThrowsOnInvalidId()
    {
        // Arrange
        var cabinCrew = _fixture.Create<CabinCrew>();
        cabinCrew.FlightId = 0;

        // Act
        var result = async () => await _sut.AddCabinCrew(cabinCrew);

        // Assert
        await result.Should().ThrowAsync<InvalidDataException>();
    }

    [Fact]
    public async Task AddLuggageReturnsOkWhenParametersAreValid()
    {
        // Arrange
        var cabinCrew = _fixture.Create<CabinCrew>();

        // Act
        await _sut.AddCabinCrew(cabinCrew);

        // Assert
        _cabinCrewRepository.Verify(x => x.AddAsync(It.IsAny<CabinCrew>()), Times.Once);
    }

    [Fact]
    public void UpdateLuggageExecutesWithValidParameters()
    {
        // Arrange
        var cabinCrew = _fixture.Create<CabinCrew>();
        _cabinCrewRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(cabinCrew);

        // Act
        _sut.EditCabinCrew(cabinCrew);

        // Assert
        _cabinCrewRepository.Verify(x => x.Update(It.IsAny<CabinCrew>()), Times.Once);
    }

    [Fact]
    public void UpdateLuggageThrowsWithInValidParameters()
    {
        // Arrange
        var cabinCrew = _fixture.Create<CabinCrew>();
        _cabinCrewRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((CabinCrew)null);

        // Act
        var result = () => _sut.EditCabinCrew(cabinCrew);

        // Assert
        result.Should().Throw<InvalidDataException>();
    }

    [Fact]
    public void DeleteLuggageExecutesWithValidParameters()
    {
        // Arrange
        var cabinCrew = _fixture.Create<CabinCrew>();
        _cabinCrewRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(cabinCrew);

        // Act
        _sut.DeleteCabinCrew(cabinCrew.Id);

        // Assert
        _cabinCrewRepository.Verify(x => x.Delete(It.IsAny<CabinCrew>()), Times.Once);
    }

    [Fact]
    public void DeleteLuggageThrowsWithInValidParameters()
    {
        // Arrange
        _cabinCrewRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((CabinCrew)null);

        // Act
        var result = () => _sut.DeleteCabinCrew(It.IsAny<int>());

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }
}