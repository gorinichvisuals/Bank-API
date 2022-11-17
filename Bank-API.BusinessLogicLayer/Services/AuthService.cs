using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository<User> userRepository;
        private readonly ITokenService tokenService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(IUserRepository<User> userRepository, 
                           ITokenService tokenService,
                           IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string?> CreateUser(UserRegistrationRequest userRequest)
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

        public async Task<string?> Login(UserLoginRequest loginRequest)
        {
            var user = await userRepository.GetUserByPhone(loginRequest.Login!);

            if (user != null && Argon2.Verify(user.PasswordHash!, loginRequest.Password!))
            {
                var token = tokenService.GenerateAccessToken(user);
                return token;
            }

            return null;
        }

        public async Task<User?> GetUser()
        {
            var authenticateUser = GetAuthenticateUser();

            if (authenticateUser != null)
            {
                var user = await userRepository.GetUserByEmail(authenticateUser.Email!);
                return user;
            }

            return null;
        }

        private User? GetAuthenticateUser()
        {
            var identity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var identityClaim = identity.Claims;

                return new User
                {
                    Email = identityClaim.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    Role = identityClaim.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
    }
}
