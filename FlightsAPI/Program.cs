using FlightsAPI.Data;
using FlightsAPI.Repositories;
using FlightsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFlightsService, FlightsService>();
builder.Services.AddScoped<ILuggageService, LuggageService>();
builder.Services.AddScoped<IPassengersService, PassengersService>();
builder.Services.AddScoped<IPlanesService, PlanesService>();
builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<IDiscountsService, DiscountsService>();
builder.Services.AddScoped<IFlightsRepository, FlightsRepository>();
builder.Services.AddScoped<ILuggageRepository, LuggageRepository>();
builder.Services.AddScoped<IPassengersRepository, PassengersRepository>();
builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
