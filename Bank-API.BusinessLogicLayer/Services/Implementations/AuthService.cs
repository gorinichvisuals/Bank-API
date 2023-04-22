namespace Bank_API.BusinessLogicLayer.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ITokenService tokenService;

    public AuthService(IUnitOfWork unitOfWork,
                       ITokenService tokenService)
    {
        this.unitOfWork = unitOfWork;
        this.tokenService = tokenService;
    }

    public async Task<APIResponse<string?>> CreateUser(UserRegistrationDTO userRequest)
    {
        APIResponse<string?> response = new ();
        bool userExists = await unitOfWork.UserRepository.Any(x => x.Email == userRequest.Email && x.Phone == userRequest.Phone);

        if (userExists)
        {
            response.ErrorMessage = "User with this email or phone number already exists";

            return response;
        }

        User newUser = new ()
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Email = userRequest.Email,
            PasswordHash = Argon2.Hash(userRequest.Password!),
            Phone = userRequest.Phone,
            BirthDate = DateTime.Parse(userRequest.BirthDate!),
            Role = Role.Customer,
        };

        await unitOfWork.UserRepository.Create(newUser);
        await unitOfWork.Save();

        var token = tokenService.GenerateAccessToken(newUser.Id, newUser.Email!, newUser.Phone!, newUser.Role);
        response.Result = token;

        return response;
    }

    public async Task<APIResponse<string?>> Login(UserLoginDTO loginRequest)
    {
        APIResponse<string?> response = new();
        var user = await unitOfWork.UserRepository.GetFirstOrDefault(x => x.Phone == loginRequest.Login!);

        if (user != null && Argon2.Verify(user.PasswordHash!, loginRequest.Password!))
        {
            var token = tokenService.GenerateAccessToken(user.Id, user.Email!, user.Phone!, user.Role);
            response.Result = token;

            return response;
        }

        response.ErrorMessage = "Incorrect credentials";

        return response;
    }
}