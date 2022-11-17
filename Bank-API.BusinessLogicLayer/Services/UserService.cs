using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Isopoh.Cryptography.Argon2;

namespace Bank_API.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> userRepository;
        private readonly IAuthService authService;

        public UserService(IUserRepository<User> userRepository, 
                           IAuthService authService)
        {
            this.userRepository = userRepository;
            this.authService = authService;
        }
      
        public async Task UpdateUser(UserUpdateRequest updateUserRequest)
        {
            var user = await authService.GetUser();
            
            if (user != null)
            {
                user.Phone = 
                    updateUserRequest!.Phone != null 
                    ? updateUserRequest.Phone 
                    : user.Phone;

                user.FirstName = 
                    updateUserRequest!.FirstName != null 
                    ? updateUserRequest.FirstName 
                    : user.FirstName;

                user.LastName = 
                    updateUserRequest!.LastName != null 
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
            }

            await userRepository.UpdateUser(user!);
        }      
    }
}
