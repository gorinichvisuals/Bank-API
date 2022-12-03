using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Enums;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository<Transaction> transactionRepository;
        private readonly IAuthService authService;
        private readonly ICardRepository<Card> cardRepository;

        public TransactionService(ITransactionRepository<Transaction> transactionRepository, 
                                  IAuthService authService,
                                  ICardRepository<Card> cardRepository)
        {
            this.transactionRepository = transactionRepository;
            this.authService = authService;
            this.cardRepository = cardRepository;
        }

        public async Task<TransactionResponse?> GetTransactionById(int? id)
        {
            User? user = await authService.GetUser();

            if (user != null)
            {
                Transaction? transactionInfo = await transactionRepository.GetTransactionById(id);

                if(transactionInfo != null)
                {
                    TransactionResponse transactionResponce = new ()
                    {
                        Amount = transactionInfo!.Amount,
                        Message = transactionInfo.Message,
                        Type = transactionInfo.Type.ToString(),
                        Peer = transactionInfo.Peer,
                        ResultingBalance = transactionInfo.ResultingBalance,
                        Date = transactionInfo.CreatedAt
                    };

                    return transactionResponce;
                }
            }

            return null;
        }

        public async Task<Response<int?>?> TransferCardToCard(CardTransferRequest request, int id)
        {
            User? user = await authService.GetUser();

            if (user != null)
            {
                Card? cardFrom = await cardRepository.GetCardById(id);

                if (cardFrom != null)
                {
                    Card? cardTo = await cardRepository.GetCardByCardNumber((long)request.CardNumber!);
                    Response<int?> response = new ();
                    
                    if(cardTo == null)
                    {
                        response.ErrorMessage = string.Format("Recipient's card is not found or unavailable");
                        return response;
                    }
                    
                    if (cardFrom.Status == CardStatus.frozen)
                    {
                        response.ErrorMessage = string.Format("Card is frozen.");
                        return response;
                    }

                    if (cardFrom.Currency != cardTo!.Currency)
                    {
                        response.ErrorMessage = string.Format("Cards have different currency.");
                        return response;
                    }

                    if (cardFrom.Balance <= request.Amount)
                    {
                        response.ErrorMessage = string.Format("Insufficient funds on the balance sheet.");
                        return response;
                    }

                    if (cardFrom.Number == request.CardNumber)
                    {
                        response.ErrorMessage = string.Format("Unable to send funds to the same card.");
                        return response;
                    }

                    var transactionFrom = new Transaction
                    {
                        CardId = id,
                        Amount = -request.Amount,
                        Message = request.Message,
                        Type = TransactionType.P2P,
                        Peer = cardTo!.User!.FirstName + " " + cardTo.User.LastName,
                        ResultingBalance = cardFrom.Balance - request.Amount,
                    };

                    var transactionTo = new Transaction
                    {
                        CardId = cardTo.Id,
                        Amount = request.Amount,
                        Message = request.Message,
                        Type = TransactionType.P2P,
                        Peer = user!.FirstName + " " + user.LastName,
                        ResultingBalance = cardTo?.Balance + request.Amount,
                    };

                    cardFrom.Balance -= request.Amount;
                    cardTo!.Balance += request.Amount;

                    await cardRepository.UpdateCard(cardFrom);
                    await cardRepository.UpdateCard(cardTo!);
                    await transactionRepository.CreateTransaction(transactionFrom, transactionTo);

                    response.Result = transactionFrom.Id;
                    return response;
                }              
            }

            return null;
        }
    }
}