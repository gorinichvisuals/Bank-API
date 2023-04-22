namespace Bank_API.BusinessLogicLayer.Services.Abstractions;

public interface ITokenService
{
    public string GenerateAccessToken(int userId, string userEmail, string userPhone, Role userRole);
}