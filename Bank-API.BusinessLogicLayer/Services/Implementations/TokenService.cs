namespace Bank_API.BusinessLogicLayer.Services.Implementations;

public class TokenService : ITokenService
{
    public readonly IConfiguration configuration;

    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateAccessToken(int userId, string userEmail, string userPhone, Role userRole)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(BankUserClaims.BankUserEmail, userEmail),
            new Claim(BankUserClaims.BankUserRole, userRole.ToString()),
            new Claim(BankUserClaims.BankUserId, userId!.ToString()!),
            new Claim(BankUserClaims.BankUserPhone, userPhone.ToString()),
        };

        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
