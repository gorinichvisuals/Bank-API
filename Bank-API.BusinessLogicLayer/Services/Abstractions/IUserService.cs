namespace Bank_API.BusinessLogicLayer.Services.Abstractions;

public interface IUserService
{    
    public Task UpdateUser(UserUpdateDTO updateUserRequest, int userId);
    public Task<APIResponse<CurrentUserDTO>> GetCurrentUser(int userId, CancellationToken cancellationToken);
}