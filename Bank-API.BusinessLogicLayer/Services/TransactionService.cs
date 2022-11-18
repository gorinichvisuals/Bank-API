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

        public async Task<int?> TransferCardToCard(CardTransferRequest request, int cardId)
        {
            User? user = await authService.GetUser();
            Card? userCard = await cardRepository.GetCardById(cardId);
            Card? toTransferCard = await cardRepository.GetCardByCardNumber((long)request.CardNumber!);

            if (user != null 
                && userCard != null 
                && toTransferCard != null 
                && userCard.Status != CardStatus.frozen 
                && userCard.Currency == toTransferCard.Currency 
                && userCard.Balance >= request.Amount
                && userCard.Number != request.CardNumber)
            {
                Transaction transactionFrom = new ()
                {
                    CardId = cardId,
                    Amount = request.Amount,
                    Message = request.Message ?? null,
                    Type = TransactionType.P2P,
                    Peer = toTransferCard.User!.FirstName + toTransferCard.User.LastName,
                    ResultingBalance = request.Amount - userCard.Balance,
                };

                Transaction transactionTo = new ()
                {
                    Id = transactionFrom.Id,
                    CardId = toTransferCard.Id,
                    Amount = request.Amount,
                    Message = request.Message ?? null, 
                    Type = TransactionType.P2P,
                    Peer = user.FirstName + user.LastName,
                    ResultingBalance = toTransferCard?.Balance + request.Amount,
                };

                await transactionRepository.CreateTransaction(transactionFrom, transactionTo);
                return transactionFrom.Id;
            }

            return null;
        }
    }
}
