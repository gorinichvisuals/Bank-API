namespace Bank_API.BusinessLogicLayer.Helpers;

public class MinAge : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return null;
        }
        
        var age = DateTime.Now.Year - Convert.ToDateTime(value).Year;

        return (age >= 18)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage = "User should be at least 18 years old.");
    }
}