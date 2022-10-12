using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Helpers
{
    public class MinAge : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (RegistrationRequest)validationContext.ObjectInstance;

            if (user.BirthDate == null)
                return new ValidationResult("Date of Birth is required.");

            var age = DateTime.Today.Year - DateTime.Parse(user.BirthDate).Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("User should be at least 18 years old.");
        }
    }
}
