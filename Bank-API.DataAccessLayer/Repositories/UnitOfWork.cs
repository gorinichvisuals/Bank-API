using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDataContext data;

        public UnitOfWork(AppDataContext data)
        {
            this.data = data;
        }

        public async Task Save()
        {
            await data.SaveChangesAsync();
        }
    }
}
