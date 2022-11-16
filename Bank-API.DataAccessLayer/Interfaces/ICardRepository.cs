using Bank_API.DataAccessLayer.Enums;

namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface ICardRepository<T>
    {
        public Task CreateCard(T card);
        public Task<T?> GetLastCard();
        public Task<ICollection<T>?> GetUserCards(int? userId, Currency currency);
        public Task<T[]?> GetUserCardsById(int? userId);
        public Task<T?> GetCardById(int id);
        public Task UpdateCard(T card);
    }
}