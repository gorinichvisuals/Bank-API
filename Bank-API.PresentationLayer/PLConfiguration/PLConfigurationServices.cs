namespace Bank_API.PresentationLayer.PLConfiguration;

public static class PLConfigurationServices
{
    public static void AddAppPLServices(this IServiceCollection services)
    {
        services.AddScoped<ISessionProvider, SessionProvider>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}