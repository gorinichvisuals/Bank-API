using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
    }
}
