namespace Bank_API.DataAccessLayer.Repositories.Implementations;

internal sealed class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(BankAPIContext context) : base(context)
    {        
    }
}