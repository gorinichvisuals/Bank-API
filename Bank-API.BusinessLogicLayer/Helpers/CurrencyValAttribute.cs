namespace Bank_API.BusinessLogicLayer.Helpers;

public class CurrencyValAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if(value != null)
            return Enum.IsDefined(typeof(Currency), value!);

        return false;
    }
}