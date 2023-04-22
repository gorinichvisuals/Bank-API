namespace Bank_API.BusinessLogicLayer.Services.Abstractions;

public interface IAuthService
{
    public Task<APIResponse<string?>> CreateUser(UserRegistrationDTO userRequest);
    public Task<APIResponse<string?>> Login(UserLoginDTO userRequest);
}