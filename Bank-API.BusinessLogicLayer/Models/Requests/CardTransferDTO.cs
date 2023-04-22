namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class CardTransferDTO
{
    [Required]
    public long Amount { get; set; }

    [Required]
    [CardNumberValidation(ErrorMessage = "Card number is invalid")]
    public long CardNumber { get; set; }

    [StringLength(128)]
    public string? Message { get; set; }
}