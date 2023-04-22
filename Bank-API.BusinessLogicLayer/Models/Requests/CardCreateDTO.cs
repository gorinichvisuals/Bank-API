namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class CardCreateDTO
{
    [Required]
    [CurrencyVal]
    public Currency Currency { get; set; }
}