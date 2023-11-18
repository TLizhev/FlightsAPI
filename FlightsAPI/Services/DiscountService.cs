using FlightsAPI.Data;

namespace FlightsAPI.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _db;

        public DiscountService(ApplicationDbContext db)
        {
            _db = db;
        }

        public int CalculateDiscount(int passengerId)
        {
            var tickets = _db.Tickets.Where(t => t.PassengerId == passengerId).ToList();

            var discount = tickets.Sum(x => x.Price) switch
            {
                < 100 => 5,
                >= 100 and < 250 => 15,
                >= 250 and < 500 => 20,
                _ => 25,
            };

            return discount;
        }
    }
}
