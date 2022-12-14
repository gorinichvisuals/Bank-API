using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Enums;
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
            return await data.Cards!
                .AsNoTracking()
                .OrderBy(c=>c.Number)
                .LastOrDefaultAsync();
        }

        public async Task<ICollection<Card>?> GetUserCards(int? userId, Currency currency)
        {
            return await data.Cards!
                .AsNoTracking()
                .Where(c => c.UserId == userId && c.Currency == currency && c.Status != CardStatus.closed)
                .ToListAsync();
        }

        public async Task<Card[]?> GetUserCardsById(int? userId)
        {
            return await data.Cards!
                .AsNoTracking()
                .Where(c => c.UserId == userId && c.Status != CardStatus.closed)
                .ToArrayAsync();
        }

        public async Task<Card?> GetCardById(int id)
        {
            return await data.Cards!
                .AsNoTracking()
                .Where(c => c.Id == id && c.Status != CardStatus.closed)
                .FirstOrDefaultAsync();
        }

        public async Task<Card?> GetCardByCardNumber(long cardNumber)
        {
            return await data.Cards!
                .AsNoTracking()
                .Include(c=>c.User)
                .Where(c => c.Number == cardNumber 
                    && c.Status != CardStatus.frozen
                    && c.Status != CardStatus.closed)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateCard(Card card)
        {
            card.UpdatedAt = DateTime.Now;
            data.Cards?.Update(card);
            await data.SaveChangesAsync();
        }
    }
}
