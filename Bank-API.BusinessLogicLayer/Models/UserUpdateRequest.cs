using Bank_API.BusinessLogicLayer.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class UserUpdateRequest
    {
        [EmailAddress]
        [StringLength(255, MinimumLength = 5)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(16, MinimumLength = 1)]
        public string? Phone { get; set; }

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
}
