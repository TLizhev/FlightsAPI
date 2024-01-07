using System;
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

public class PassengerServiceTests
{
    private readonly Mock<IPassengersRepository> _passengersRepository;
    private readonly PassengersService _sut;
    private readonly Fixture _fixture;

    public PassengerServiceTests()
    {
        _passengersRepository = new Mock<IPassengersRepository>();
        _sut = new PassengersService(_passengersRepository.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void GetPassengersReturnsCollection()
    {
        // Arrange
        var passengers = _fixture.CreateMany<Passenger>().ToList();
        _passengersRepository.Setup(x => x.GetAll()).Returns(passengers);

        // Act
        var result = _sut.GetPassengers();

        // Assert
        result.Should().BeEquivalentTo(passengers);
    }

    [Fact]
    public void GetPassengerByIdReturnsPassenger()
    {
        // Arrange
        var passenger = _fixture.Create<Passenger>();
        _passengersRepository.Setup(x => x.GetById(passenger.Id)).Returns(passenger);

        // Act
        var result = _sut.GetPassenger(passenger.Id);

        // Assert
        result.Should().BeEquivalentTo(passenger);
    }

    [Fact]
    public void GetPassengerByIdThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _passengersRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Passenger());

        // Act
        var result = () => _sut.GetPassenger(999);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task AddPassengerThrowsWhenPassengerAlreadyExists()
    {
        // Arrange
        var passengers = _fixture.CreateMany<Passenger>().ToList();
        passengers[0].Id = 1;

        var passenger = _fixture.Build<Passenger>().With(x => x.Id, 1).Create();
        _passengersRepository.Setup(x => x.GetById(1)).Returns(passenger);

        // Act
        var result = async () => await _sut.AddPassenger(passenger);

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AddPassengerWorksWhenParametersAreValid()
    {
        // Arrange
        var passenger = _fixture.Create<Passenger>();

        // Act
        await _sut.AddPassenger(passenger);

        // Assert
        _passengersRepository.Verify(x => x.AddAsync(It.IsAny<Passenger>()), Times.Once);
    }

    [Fact]
    public void EditPassengerThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _passengersRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Passenger());

        var passenger = _fixture.Build<Passenger>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.EditPassenger(passenger);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void EditPassengerWorksWhenParametersAreValid()
    {
        // Arrange
        var passenger = _fixture.Create<Passenger>();
        _passengersRepository.Setup(x => x.GetById(passenger.Id)).Returns(passenger);

        // Act
        _sut.EditPassenger(passenger);

        // Assert
        _passengersRepository.Verify(x => x.Update(It.IsAny<Passenger>()), Times.Once);
    }

    [Fact]
    public void DeletePassengerThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _passengersRepository.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Passenger());

        var passenger = _fixture.Build<Passenger>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.DeletePassenger(passenger.Id);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void DeletePassengerWorksWhenParametersAreValid()
    {
        // Arrange
        var passenger = _fixture.Create<Passenger>();
        _passengersRepository.Setup(x => x.GetById(passenger.Id)).Returns(passenger);

        // Act
        _sut.DeletePassenger(passenger.Id);

        // Assert
        _passengersRepository.Verify(x => x.Delete(It.IsAny<Passenger>()), Times.Once);
    }
}
