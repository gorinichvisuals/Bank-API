using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private AppDataContext data;

        public UserRepository(AppDataContext data)
        {
            this.data = data;
        }

        public async Task CreateUser(User user)
        {
            await data.Users.AddAsync(user);
        }

        public async Task<User> GetUserByEmailAndPhone(string email, string phone)
        {
            return await data.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && u.Phone == phone);
        }
    }
}
