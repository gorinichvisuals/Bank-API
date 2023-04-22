namespace Bank_API.BusinessLogicLayer.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task UpdateUser(UserUpdateDTO updateUserRequest, int userId)
    {
        var user = await unitOfWork.UserRepository.GetFirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            user.Phone =
                updateUserRequest!.Phone != null
                ? updateUserRequest.Phone
                : user.Phone;

            user.FirstName =
                updateUserRequest!.FirstName != null
                ? updateUserRequest.FirstName
                : user.FirstName;

            user.LastName =
                updateUserRequest!.LastName != null
                ? updateUserRequest.LastName
                : user.LastName;

            user.BirthDate =
                updateUserRequest.BirthDate! != null
                ? DateTime.Parse(updateUserRequest.BirthDate!)
                : user.BirthDate;

            user.PasswordHash =
                updateUserRequest.Password != null
                ? Argon2.Hash(updateUserRequest.Password!)
                : user.PasswordHash;
        }

        unitOfWork.UserRepository.Update(user!);
        await unitOfWork.Save();
    }

    public async Task<APIResponse<CurrentUserDTO>> GetCurrentUser(int userId, CancellationToken cancellationToken)
    {
        APIResponse<CurrentUserDTO> response = new ();
        CurrentUserDTO? user = await unitOfWork.UserRepository
            .GetFirstOrDefaultWithSelect(
            x => x.Id == userId, 
            x => new CurrentUserDTO
            {
                Email = x.Email,
                Phone = x.Phone,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirtDate = x.BirthDate.ToString("d"),
            }, 
            cancellationToken);

        if (user == null)
        {
            response.ErrorMessage = "Unauthorize";

            return response;
        }

        response.Result = user;

        return response;
    }
}