using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class CardRepository : ICardRepository<Card>
    {
        public readonly AppDataContext data;

        public CardRepository(AppDataContext data)
        {
            this.data = data;
        }

        public async Task CreateCard(Card card)
        {
            await data.Cards!.AddAsync(card);
            await data.SaveChangesAsync();
        }

        public async Task<Card?> GetLastCard()
        {
            return await data.Cards!.AsNoTracking().OrderBy(c=>c.Number).LastOrDefaultAsync();
        }
    }
}
