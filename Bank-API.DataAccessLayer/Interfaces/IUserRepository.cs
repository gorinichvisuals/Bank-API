namespace Bank_API.DataAccessLayer.Interfaces
{
    public interface IUserRepository<T>
    {
        public Task CreateUser(T user);
        public Task<T> GetUserByEmailAndPhone(string userEmail, string userPhone);
        public Task<T> GetUserByPhone(string userPhone);    
        public Task<T> GetUserById(int userId);
    }
}
