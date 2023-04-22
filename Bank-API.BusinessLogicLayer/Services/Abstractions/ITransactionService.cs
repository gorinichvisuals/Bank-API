namespace Bank_API.BusinessLogicLayer.Services.Abstractions;

public interface ITransactionService
{
    public Task<APIResponse<TransactionDTO>> GetTransactionById(int transactionId, CancellationToken cancellationToken);
    public Task<APIResponse<int>> TransferCardToCard(CardTransferDTO request, int transactionId);
}