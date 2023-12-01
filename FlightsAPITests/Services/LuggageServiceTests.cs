using AutoFixture;
using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using FlightsAPI.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FlightsAPITests.Services
{
    public class LuggageServiceTests
    {
        private readonly LuggageService _sut;
        private readonly Mock<ILuggageRepository> _luggageRepository;

        public LuggageServiceTests()
        {
            _luggageRepository = new();
            _sut = new(_luggageRepository.Object);
        }

        [Fact]
        public void GetAllReturnsResult()
        {
            Fixture luggageFixture = new Fixture();
            var list = luggageFixture.CreateMany<Luggage>(3).ToList();
            _luggageRepository.Setup(x => x.GetAll()).Returns(list);

            var result = _sut.GetLuggages();

            result.Should().BeEquivalentTo(list);
        }
    }
}
