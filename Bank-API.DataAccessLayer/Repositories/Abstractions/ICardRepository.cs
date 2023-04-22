namespace Bank_API.DataAccessLayer.Repositories.Abstractions;

public interface ICardRepository : IBaseRepository<Card>
{
    public Task UpdateCardStatus(Expression<Func<Card, bool>> predicate, CardStatus cardStatus);
}