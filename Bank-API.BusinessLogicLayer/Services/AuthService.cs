using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Isopoh.Cryptography.Argon2;

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

        public async Task<string> CreateUser(RegistrationRequest userRequest)
        {
            var user = await userRepository.GetUserByEmailAndPhone(userRequest.Email, userRequest.Phone);
     
            if (user == null)
            {
                if (AgeVerification(userRequest.BirthDate))
                {
                    var createUser = new User
                    {
                        FirstName = userRequest.FirstName,
                        LastName = userRequest.LastName,
                        Email = userRequest.Email,
                        PasswordHash = Argon2.Hash(userRequest.Password),
                        Phone = userRequest.Phone,
                        BirthDate = BirthTransformation(userRequest.BirthDate)
                    };

                    await userRepository.CreateUser(createUser);

                    var token = tokenService.GenerateAccessToken(createUser);
                    return token;
                }
            }

            return null;
        }

        private bool AgeVerification(string birthDay)
        {
            DateTime convertBirthDay = BirthTransformation(birthDay);
            int interval = new DateTime(DateTime.Now.Ticks - convertBirthDay.Ticks).Year;

            return interval >= 18;
        }

        private DateTime BirthTransformation(string birthDay)
        {
            return DateTime.Parse(birthDay);
        }
    }
}
