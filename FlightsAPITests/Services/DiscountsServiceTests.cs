﻿using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Moq;
using System.Linq;
using AutoFixture;
using FlightsAPI.Repositories;
using FluentAssertions;
using Xunit;

namespace FlightsAPITests.Services;

public class DiscountsServiceTests
{
    private readonly Mock<ITicketsRepository> _ticketsRepository;
    private readonly DiscountService _sut;
    private readonly Fixture _fixture;

    public DiscountsServiceTests()
    {
        _fixture = new Fixture();
        _ticketsRepository = new Mock<ITicketsRepository>();
        _sut = new DiscountService(_ticketsRepository.Object);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(99, 5)]
    [InlineData(100, 15)]
    [InlineData(250, 20)]
    [InlineData(500, 25)]
    public void ItShouldCalculateDiscountCorrectly(decimal price, int discount)
    {
        // Arrange
        var passengerTickets = _fixture.CreateMany<Ticket>().ToList();
        passengerTickets[0].PassengerId = 5;
        passengerTickets[0].Price = price;
        _ticketsRepository.Setup(x => x.GetAll()).Returns(passengerTickets);

        // Act
        var result = _sut.CalculateDiscount(passengerTickets[0].PassengerId);

        // Assert
        result.Should().Be(discount);
    }
}