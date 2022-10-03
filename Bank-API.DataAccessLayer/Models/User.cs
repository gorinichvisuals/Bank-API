using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_API.DataAccessLayer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        [StringLength(255, MinimumLength = 5)]
        [Required]
        public string Email { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(64, MinimumLength = 1)]
        [Required]
        public string LastName { get; set; }

        [Phone]
        [StringLength(16, MinimumLength = 1)]
        [Required]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
