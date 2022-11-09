using Bank_API.DataAccessLayer.DataContext;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_API.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly AppDataContext data;

        public UserRepository(AppDataContext data)
        {
            this.data = data;
        }

        public async Task CreateUser(User user)
        {
            await data.Users!.AddAsync(user);
            await data.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            data.Users?.Update(user);
            await data.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAndPhone(string? email, string? phone)
        {
            return await data.Users!
                .AsNoTracking()
                .Include(u => u.Cards)
                .FirstOrDefaultAsync(u => u.Email == email || u.Phone == phone);
        }

        public async Task<User?> GetUserByPhone(string? phone)
        {
            return await data.Users!
                .AsNoTracking()
                .Include(u => u.Cards)
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<User?> GetUserByEmail(string? email)
        {
            return await data.Users!
                .AsNoTracking()
                .Include(u => u.Cards)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
