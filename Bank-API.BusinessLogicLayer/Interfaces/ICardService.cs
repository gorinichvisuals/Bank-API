using Bank_API.BusinessLogicLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface ICardService
    {
        public Task<int?> CreateCard(CardCreateRequest request);
        public Task<CardResponse[]?> GetUserCards();
        public Task<bool> ChangeCardStatus(int id);
    }
}