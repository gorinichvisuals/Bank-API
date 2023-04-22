namespace Bank_API.DataAccessLayer.Repositories.Implementations;

internal sealed class CardRepository : BaseRepository<Card>, ICardRepository
{
    public CardRepository(BankAPIContext context) : base(context)
    {       
    }

    public async Task UpdateCardStatus(Expression<Func<Card, bool>> predicate, CardStatus cardStatus)
    {
        await context.Cards!
            .Where(predicate)
            .ExecuteUpdateAsync(x =>x.SetProperty(x =>x.Status, cardStatus).SetProperty(x=>x.UpdatedAt, DateTime.Now));
    }
}