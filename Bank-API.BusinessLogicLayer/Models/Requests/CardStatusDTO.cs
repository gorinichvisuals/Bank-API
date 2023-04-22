namespace Bank_API.BusinessLogicLayer.Models.Requests;

public class CardStatusDTO
{
    [Required]
    public bool FreezeCard { get; set; }
}