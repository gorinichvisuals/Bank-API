using Bank_API.BusinessLogicLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface ITransactionService
    {
        public Task<TransactionResponse?> GetTransactionById(int? id);
        public Task<(int?, string?)> TransferCardToCard(CardTransferRequest request, int id);
    }
}
