using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUser();
    }
}
