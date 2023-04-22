namespace Bank_API.BusinessLogicLayer.Services.Abstractions;

public interface ICardService
{
    public Task<APIResponse<int>> CreateCard(CardCreateDTO cardRequest, int userId);
    public Task<ICollection<CardDTO>> GetUserCards(int userId, CancellationToken cancellationToken);
    public Task<APIResponse<bool>> ChangeCardStatus(bool freezeCard, int cardId);
}