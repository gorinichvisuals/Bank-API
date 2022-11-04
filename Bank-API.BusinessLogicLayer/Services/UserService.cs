using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IUserRepository<User> userRepository,
                           IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<User?> GetUser()
        {
            var user = GetAuthenticateUser();

            if (user != null)
            {
                var userData = await userRepository.GetUserByEmail(user.Email!);
                return userData;
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
                    Role = identityClaim.FirstOrDefault(c=> c.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
    }
}
