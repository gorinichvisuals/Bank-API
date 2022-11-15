using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class UserLoginRequest
    {
        [Required]
        [StringLength(16, MinimumLength = 1)]
        [Phone]
        public string? Login { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
