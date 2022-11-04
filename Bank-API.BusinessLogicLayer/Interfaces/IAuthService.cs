using Bank_API.BusinessLogicLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        public Task<string?> CreateUser(RegistrationRequest userRequest);
        public Task<string?> Login(LoginRequest userRequest);
    }
}
