namespace Bank_API.PresentationLayer.Services.Abstractions;

public interface ISessionProvider
{
    public int GetUserId();
    public string GetUserEmail();
    public string GetUserRole();
    public string GetUserPhone();
    public Task<string> GetUserToken();
}