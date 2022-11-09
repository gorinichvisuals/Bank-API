namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface ICardRepository<T>
    {
        public Task CreateCard(T card);
        public Task<T?> GetLastCard(); 
    }
}
