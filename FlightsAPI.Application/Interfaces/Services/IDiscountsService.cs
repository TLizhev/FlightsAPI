namespace FlightsAPI.Application.Interfaces.Services;

public interface IDiscountsService
{
    int CalculateDiscount(int passengerId);
}