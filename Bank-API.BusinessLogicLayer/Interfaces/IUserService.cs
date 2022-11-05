using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUser();
        public Task UpdateUser(UserUpdateRequest updateRequest);
    }
}
