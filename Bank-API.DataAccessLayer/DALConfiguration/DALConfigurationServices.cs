namespace Bank_API.DataAccessLayer.DALConfiguration;

public static class DALConfigurationServices
{
    public static void AddAppDALServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}