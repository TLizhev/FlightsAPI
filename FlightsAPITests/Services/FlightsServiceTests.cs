//using System;
//using AutoFixture;
//using FlightsAPI.Data;
//using FlightsAPI.Data.Models;
//using FlightsAPI.Services;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;

//namespace FlightsAPITests.Services
//{

//    public class FlightsServiceTests
//    {
//        private readonly FlightsService _sut;
//        private readonly Mock<ApplicationDbContext> _db;
//        private readonly Fixture _fixture;

//        public FlightsServiceTests()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
//            _fixture = new Fixture();
//            _db = new Mock<ApplicationDbContext>(options);
//            _sut = new FlightsService(_db.Object);
//        }

//        [Fact]
//        public void ItReturnsFlights()
//        {
//            // Arrange
//            var flights = new Mock<DbSet<Flight>>();
//            _db.Setup(x => x).Returns(flights.Object);

//            // Act
//            var result = _sut.GetFlights();

//            // Assert
//            result.Should().BeEquivalentTo(result);
//        }
//    }
//}
