namespace Bank_API.BusinessLogicLayer.BLLConfiguration;

public static class BLLConfigurationServices
{
    public static void AddAppBLLServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}