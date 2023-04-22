namespace Bank_API.BusinessLogicLayer.Models.Responses;

public class CurrentUserDTO
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirtDate { get; set; }
}