namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class UserUpdateDTO
{
    [EmailAddress]
    [StringLength(255, MinimumLength = 5)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(16, MinimumLength = 1)]
    public string? Phone { get; set; }

    [StringLength(64, MinimumLength = 1)]
    public string? Password { get; set; }

    [StringLength(64, MinimumLength = 1)]
    public string? FirstName { get; set; }

    [StringLength(64, MinimumLength = 1)]
    public string? LastName { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [MinAge]
    public string? BirthDate { get; set; }
}