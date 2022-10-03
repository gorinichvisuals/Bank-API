using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private DataContext.AppDataContext data;

        public UserRepository(DataContext.AppDataContext data)
        {
            this.data = data;
        }
    }
}
