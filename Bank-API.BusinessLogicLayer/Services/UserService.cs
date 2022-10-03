using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> userRepository;

        public UserService(IUserRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}
