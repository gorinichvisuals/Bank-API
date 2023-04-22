namespace Bank_API.DataAccessLayer.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [EmailAddress]
    [StringLength(255, MinimumLength = 5)]
    [Required]
    public string? Email { get; set; }

    [StringLength(64, MinimumLength = 1)]
    [Required]
    public string? FirstName { get; set; }

    [StringLength(64, MinimumLength = 1)]
    [Required]
    public string? LastName { get; set; }

    [Phone]
    [StringLength(16, MinimumLength = 1)]
    [Required]
    public string? Phone { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedAt { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Role Role { get; set; }

    public ICollection<Card>? Cards { get; set; }
}