using System.Diagnostics.CodeAnalysis;
using FlightsAPI.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightsAPI.Infra;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Flight> Flights { get; set; } = null!;
    public DbSet<Luggage> Luggages { get; set; } = null!;
    public DbSet<LuggageType> LuggageTypes { get; set; } = null!;
    public DbSet<Passenger> Passengers { get; set; } = null!;
    public DbSet<Plane> Planes { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=.;Database=Airport;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}