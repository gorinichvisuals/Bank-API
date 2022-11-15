using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class TransactionRepository : ITransactionRepository<Transaction>
    {
        private readonly AppDataContext data;

        public TransactionRepository(AppDataContext data)
        {
            this.data = data;
        }

        public async Task<Transaction?> GetTransactionById(int? id)
        {
            return await data.Transactions!
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(); 
        }
    }
}
