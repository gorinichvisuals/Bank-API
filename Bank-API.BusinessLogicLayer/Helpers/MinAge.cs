using Bank_API.BusinessLogicLayer.Models;
using Bank_API.DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bank_API.BusinessLogicLayer.Helpers
{
    public class MinAge : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var user = (RegistrationRequest)validationContext.ObjectInstance;

            var age = DateTime.Today.Year - DateTime.Parse(user.BirthDate!).Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage = "User should be at least 18 years old.");
        }
    }
}
