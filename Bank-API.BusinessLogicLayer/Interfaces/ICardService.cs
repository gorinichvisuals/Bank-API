using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface ICardService
    {
        public Task<int?> CreateCard(CardCreateRequest request);
        public Task<CardResponce[]?> GetUserCards();
    }
}
