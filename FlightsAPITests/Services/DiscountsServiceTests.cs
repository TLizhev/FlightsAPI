//using System;
//using FlightsAPI.Data;
//using FlightsAPI.Data.Models;
//using FlightsAPI.Services;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using FluentAssertions;
//using Xunit;

//namespace FlightsAPITests.Services;

//public class DiscountsServiceTests
//{
//    private readonly TicketRepositoryMock _ticketService;
//    private readonly DiscountService _sut;
//    private readonly Mock<ITicketService> _ticketServiceMock;
//    private readonly Mock<ApplicationDbContext> _context;
//    private readonly Mock<DbContextOptions> _options;

//    public DiscountsServiceTests()
//    {
//        _ticketServiceMock = new Mock<ITicketService>();
//        _ticketService = new TicketRepositoryMock();
        
//        _sut = new DiscountService(DbContextMock.GetMock(_ticketService.));
//    }

//    [Fact]
//    public void ItShouldCalculateDiscountCorrectly()
//    {
//        // Arrange


//        // Act

//        // Assert

//    }
//}

//public class DbContextMock
//{
//    public static TContext GetMock<TData, TContext>(List<TData> lstData, Expression<Func<TContext, DbSet<TData>>> dbSetSelectionExpression) where TData : class where TContext : ApplicationDbContext
//    {
//        IQueryable<TData> lstDataQueryable = lstData.AsQueryable();
//        Mock<DbSet<TData>> dbSetMock = new Mock<DbSet<TData>>();
//        Mock<TContext> dbContext = new Mock<TContext>();

//        dbSetMock.As<IQueryable<TData>>().Setup(s => s.Provider).Returns(lstDataQueryable.Provider);
//        dbSetMock.As<IQueryable<TData>>().Setup(s => s.Expression).Returns(lstDataQueryable.Expression);
//        dbSetMock.As<IQueryable<TData>>().Setup(s => s.ElementType).Returns(lstDataQueryable.ElementType);
//        dbSetMock.As<IQueryable<TData>>().Setup(s => s.GetEnumerator()).Returns(() => lstDataQueryable.GetEnumerator());
//        dbSetMock.Setup(x => x.Add(It.IsAny<TData>())).Callback<TData>(lstData.Add);
//        dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<TData>>())).Callback<IEnumerable<TData>>(lstData.AddRange);
//        dbSetMock.Setup(x => x.Remove(It.IsAny<TData>())).Callback<TData>(t => lstData.Remove(t));
//        dbSetMock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<TData>>())).Callback<IEnumerable<TData>>(ts =>
//        {
//            foreach (var t in ts) { lstData.Remove(t); }
//        });


//        dbContext.Setup(dbSetSelectionExpression).Returns(dbSetMock.Object);

//        return dbContext.Object;
//    }
//}

//public class TicketRepositoryMock
//{
//    public static ITicketService GetMock()
//    {
//        List<Ticket> tickets = GenerateTestData();
//        ApplicationDbContext dbContextMock = DbContextMock.GetMock<Ticket, ApplicationDbContext>(tickets, x => x.Tickets);
//        return new TicketService(dbContextMock);
//    }

//    private static List<Ticket> GenerateTestData()
//    {
//        var tickets = new List<Ticket>()
//            {
//                new Ticket
//                {
//                    Id = 1,
//                    PassengerId = 1,
//                    FlightId = 1,
//                    LuggageId = 1,
//                    Price = 99
//                }
//            };
//        return tickets;
//    }
//}
//}