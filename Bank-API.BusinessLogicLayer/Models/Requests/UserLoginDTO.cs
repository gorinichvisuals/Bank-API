namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class UserLoginDTO
{
    [Required]
    [StringLength(16, MinimumLength = 1)]
    [Phone]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }
}