namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface IUserRepository<T>
    {
        public Task CreateUser(T user);
        public Task<T?> GetUserByEmailAndPhone(string email, string phone);
        public Task<T?> GetUserByPhone(string phone);
        public Task<T?> GetUserByEmail(string email);
        public Task UpdateUser(T user);
    }
}
