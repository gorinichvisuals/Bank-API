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

        public async Task<(int?, string?)> TransferCardToCard(CardTransferRequest request, int id)
        {
            User? user = await authService.GetUser();
            Card? cardFrom = await cardRepository.GetCardById(id);
            Card? cardTo = await cardRepository.GetCardByCardNumber((long)request.CardNumber!);

            if (user != null && cardFrom != null && cardTo != null)
            {
                if(cardFrom.Status == CardStatus.frozen)
                {
                    var message = string.Format("Card is frozen.");
                    return (default, message);
                }

                if(cardFrom.Currency != cardTo.Currency)
                {
                    var message = string.Format("Cards have different currency.");
                    return (default, message);
                }

                if(cardFrom.Balance <= request.Amount)
                {
                    var message = string.Format("Insufficient funds on the balance sheet.");
                    return (default, message);
                }

                if (cardFrom.Number == request.CardNumber)
                {
                    var message = string.Format("Unable to send funds to the same card.");
                    return (default, message);
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

                return (transactionFrom.Id, default);
            }

            return (null, default);
        }
    }
}