using Bank_API.DataAccessLayer.Models;

namespace Bank_API.BusinessLogicLayer.Interfaces
{
    public interface ITokenService
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();
    }
}
