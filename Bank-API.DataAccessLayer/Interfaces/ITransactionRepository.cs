﻿namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface ITransactionRepository<T>
    {
        public Task<T?> GetTransactionById(int? id);
        public Task<T[]?> GetTransactionList(int? cardId);
    }
}
