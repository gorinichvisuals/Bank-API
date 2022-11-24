using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
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
                    TransactionResponse? transactionResponce = new ()
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

        public async Task<List<TransactionResponse>?> GetTransactionsList(TransactionQueryParams requestParams, int cardId)
        {
            User? user = await authService.GetUser();
            Card? card = await cardRepository.GetCardById(cardId);

            if(user != null && card != null)
            {
                List<Transaction>? transactions = await transactionRepository
                    .GetTransactionList(card!.Id, requestParams.SortBy!, requestParams.SortDirection!);

                List<TransactionResponse> transactionsList = transactions!
                                    .Select(t => new TransactionResponse
                                    {
                                        Id = t.Id,
                                        Amount = t.Amount,
                                        Peer = t.Peer,
                                        Message = t.Message,
                                        Type = t.Type.ToString(),
                                        Date = t.CreatedAt
                                    })
                                    .Take(requestParams.Limit)
                                    .ToList();

                return transactionsList;
            }

            return null;
        }
    }
}