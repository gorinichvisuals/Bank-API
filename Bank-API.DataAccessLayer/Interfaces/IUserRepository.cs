namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface IUserRepository<T>
    {
        public Task CreateUser(T user);
        public Task<T> GetUserByEmailAndPhone(string email, string phone);
    }
}
