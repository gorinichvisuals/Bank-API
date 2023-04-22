namespace Bank_API.DataAccessLayer.Repositories.Implementations;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly BankAPIContext bankAPIContext;
    public IUserRepository UserRepository { get; }
    public ICardRepository CardRepository { get; }
    public ITransactionRepository TransactionRepository { get; }

    public UnitOfWork(
        BankAPIContext bankAPIContext, 
        IUserRepository userRepository, 
        ICardRepository cardRepository, 
        ITransactionRepository transactionRepository)
    {
        this.bankAPIContext = bankAPIContext;
        UserRepository = userRepository;
        CardRepository = cardRepository;
        TransactionRepository = transactionRepository;
    }

    public async Task Save()
    {
        await bankAPIContext.SaveChangesAsync();
    }
}