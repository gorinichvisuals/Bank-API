using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        
        public async Task UpdateUser(UserUpdateRequest updateUserRequest)
        {
            var authenticateUser = GetAuthenticateUser();

            if(authenticateUser != null)
            {
                var user = await userRepository.GetUserByEmail(authenticateUser.Email!);

                if (user != null)
                {
                    user.Phone = 
                        updateUserRequest.Phone != null 
                        ? updateUserRequest.Phone 
                        : user.Phone;

                    user.FirstName = 
                        updateUserRequest.FirstName != null 
                        ? updateUserRequest.FirstName 
                        : user.FirstName;

                    user.LastName = 
                        updateUserRequest.LastName != null 
                        ? updateUserRequest.LastName 
                        : user.LastName;

                    user.BirthDate = 
                        updateUserRequest.BirthDate! != null 
                        ? DateTime.Parse(updateUserRequest.BirthDate!) 
                        : user.BirthDate;

                    user.PasswordHash = 
                        updateUserRequest.Password != null 
                        ? Argon2.Hash(updateUserRequest.Password!) 
                        : user.PasswordHash;

                    user.UpdatedAt = DateTime.Now;
                }

                await userRepository.UpdateUser(user!);
            }
           
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
