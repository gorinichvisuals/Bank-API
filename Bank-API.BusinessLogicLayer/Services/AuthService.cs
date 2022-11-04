using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Isopoh.Cryptography.Argon2;
using System.Net;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository<User> userRepository;
        private readonly ITokenService tokenService;

        public AuthService(IUserRepository<User> userRepository, 
                           ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public async Task<string?> CreateUser(RegistrationRequest userRequest)
        {
            var user = await userRepository.GetUserByEmailAndPhone(userRequest.Email!, userRequest.Phone!);

            if (user == null)
            {
                var createUser = new User
                {
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    Email = userRequest.Email,
                    PasswordHash = Argon2.Hash(userRequest.Password!),
                    Phone = userRequest.Phone,
                    BirthDate = DateTime.Parse(userRequest.BirthDate!),
                    Role = "User",
                };

                await userRepository.CreateUser(createUser);

                var token = tokenService.GenerateAccessToken(createUser);
                return token;
            }

            return null;
        }

        public async Task<string?> Login(LoginRequest loginRequest)
        {
            var user = await userRepository.GetUserByPhone(loginRequest.Login!);

            if (user != null && Argon2.Verify(user.PasswordHash!, loginRequest.Password!))
            {
                var token = tokenService.GenerateAccessToken(user);
                return token;
            }

            return null;
        }
    }
}
