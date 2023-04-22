namespace Bank_API.BusinessLogicLayer.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<APIResponse<TransactionDTO>> GetTransactionById(int transactionId, CancellationToken cancellationToken)
    {
        APIResponse<TransactionDTO> response = new ();
        bool transactionExists = await unitOfWork.TransactionRepository.Any(x => x.Id == transactionId, cancellationToken);

        if (transactionExists)
        {
            TransactionDTO? transaction = await unitOfWork.TransactionRepository
                .GetFirstOrDefaultWithSelect(x => x.Id == transactionId, x => new TransactionDTO()
                {
                    Amount = x.Amount,
                    Message = x.Message,
                    Type = x.Type.ToString(),
                    Peer = x.Peer,
                    ResultingBalance = x.ResultingBalance,
                    Date = x.CreatedAt
                }, 
                cancellationToken);

            response.Result = transaction;

            return response;
        }

        response.ErrorMessage = "Transaction not found or unavaulable";

        return response;
    }

    public async Task<APIResponse<int>> TransferCardToCard(CardTransferDTO request, int cardId)
    {
        APIResponse<int> response = new();
        Card? cardFrom = await unitOfWork.CardRepository.GetFirstOrDefault(x => x.Id == cardId);

        if (cardFrom == null)
        {
            response.ErrorMessage = "Card is not found or unavailable";
            return response;
        }

        Card? cardTo = await unitOfWork.CardRepository.GetFirstOrDefault(x => x.Number == request.CardNumber);

        if (cardTo == null)
        {
            response.ErrorMessage = "Recipient's card is not found or unavailable";
            return response;
        }

        if (cardFrom!.Status != CardStatus.active)
        {
            response.ErrorMessage = "Card is not active.";
            return response;
        }

        if (cardFrom.Currency != cardTo!.Currency)
        {
            response.ErrorMessage = "Cards have different currency.";
            return response;
        }

        if (cardFrom.Balance <= request.Amount)
        {
            response.ErrorMessage = "Insufficient funds on the balance sheet.";
            return response;
        }

        if (cardFrom.Number == request.CardNumber)
        {
            response.ErrorMessage = "Unable to send funds to the same card.";
            return response;
        }

        Transaction transactionFrom = new ()
        {
            CardId = cardId,
            Amount = -request.Amount,
            Message = request.Message,
            Type = TransactionType.P2P,
            Peer = $"{cardTo!.User!.FirstName} {cardTo.User.LastName}",
            ResultingBalance = cardFrom.Balance - request.Amount,
        };

        Transaction transactionTo = new ()
        {
            CardId = cardTo.Id,
            Amount = request.Amount,
            Message = request.Message,
            Type = TransactionType.P2P,
            Peer = $"{cardFrom.User!.FirstName!} {cardFrom.User!.LastName!}",
            ResultingBalance = cardTo.Balance + request.Amount,
        };

        cardFrom.Balance -= request.Amount;
        cardTo!.Balance += request.Amount;

        unitOfWork.CardRepository.Update(cardFrom);
        unitOfWork.CardRepository.Update(cardTo!);
        await unitOfWork.TransactionRepository.Create(transactionFrom);
        await unitOfWork.TransactionRepository.Create(transactionTo);
        await unitOfWork.Save();

        response.Result = transactionFrom.Id;
        return response;
    }
}