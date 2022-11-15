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
        private readonly IUserService userService;

        public TransactionService(ITransactionRepository<Transaction> transactionRepository, 
                                  IAuthService authService, 
                                  IUserService userService)
        {
            this.transactionRepository = transactionRepository;
            this.authService = authService;
            this.userService = userService;
        }

        public async Task<TransactionResponse?> GetTransactionById(int? id)
        {
            var user = await userService.GetUser();

            if (user != null)
            {
                var transactionInfo = await transactionRepository.GetTransactionById(id);

                var transactionResponce = new TransactionResponse
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

            return null;
        }
    }
}
