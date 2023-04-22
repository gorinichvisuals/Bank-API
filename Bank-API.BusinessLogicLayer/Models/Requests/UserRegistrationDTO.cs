namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class UserRegistrationDTO
{
    [Required]
    [EmailAddress]
    [StringLength(255, MinimumLength = 5)]
    public string? Email { get; set; }

    [Required]
    [Phone]
    [StringLength(16, MinimumLength = 1)]
    public string? Phone { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [MinAge]
    public string? BirthDate { get; set; }
}