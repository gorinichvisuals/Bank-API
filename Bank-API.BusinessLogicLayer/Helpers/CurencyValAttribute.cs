using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Helpers
{
    public class CurencyValAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string currency)
            {
                if (currency == "UAH" || currency == "USD" || currency == "EUR")
                    return true;
                else 
                    ErrorMessage = "Incorrect input";
            } 

            return false;
        }
    }
}
