using Bank_API.BusinessLogicLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        public Task<string?> CreateUser(UserRegistrationRequest userRequest);
        public Task<string?> Login(UserLoginRequest userRequest);
    }
}
