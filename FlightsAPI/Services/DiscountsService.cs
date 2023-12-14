using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class DiscountsService : IDiscountsService
    {
        private readonly ITicketsRepository _ticketsRepository;

        public DiscountsService(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public int CalculateDiscount(int passengerId)
        {
            var tickets = _ticketsRepository.GetAll()
                .Where(t => t.PassengerId == passengerId)
                .ToList();

            var discount = tickets.Sum(x => x.Price) switch
            {
                0 => 0,
                >= 1 and < 100 => 5,
                >= 100 and < 250 => 15,
                >= 250 and < 500 => 20,
                _ => 25,
            };

            return discount;
        }
    }
}
