using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        public Task Save();
    }
}
