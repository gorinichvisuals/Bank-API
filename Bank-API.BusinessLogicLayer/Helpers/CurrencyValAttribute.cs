using Bank_API.DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Helpers
{
    public class CurrencyValAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return Enum.IsDefined(typeof(Currency), value!);
        }
    }
}
