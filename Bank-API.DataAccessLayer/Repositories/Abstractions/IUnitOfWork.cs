namespace Bank_API.DataAccessLayer.Repositories.Abstractions;

public interface IUnitOfWork
{
    public ICardRepository CardRepository { get; }
    public IUserRepository UserRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public Task Save();
}