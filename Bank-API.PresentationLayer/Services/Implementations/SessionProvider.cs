namespace Bank_API.PresentationLayer.Services.Implementations;

internal sealed class SessionProvider : ISessionProvider
{
    private readonly IHttpContextAccessor contextAccessor;

    public SessionProvider(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public async Task<string> GetUserToken()
    {
        var accessToken = await contextAccessor.HttpContext!.GetTokenAsync("access_token");
        return accessToken!;
    }

    public int GetUserId()
    {
        var context = int.TryParse(contextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == BankUserClaims.BankUserId)!.Value,
        out var userId);

        return userId;
    }

    public string GetUserPhone()
    {
        var context = contextAccessor.HttpContext;
        return context!.User.Claims.FirstOrDefault(u => u.Type == BankUserClaims.BankUserPhone)!.Value;
    }

    public string GetUserEmail()
    {
        var context = contextAccessor.HttpContext;
        return context!.User!.Claims.FirstOrDefault(x => x.Type == BankUserClaims.BankUserEmail)!.Value;
    }

    public string GetUserRole()
    {
        var context = contextAccessor.HttpContext;
        return context!.User!.Claims.FirstOrDefault(x => x.Type == BankUserClaims.BankUserRole)!.Value;
    }
}