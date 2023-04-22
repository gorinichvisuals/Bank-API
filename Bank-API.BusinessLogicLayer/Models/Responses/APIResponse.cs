namespace Bank_API.BusinessLogicLayer.Models.Responses;

public class APIResponse<T>
{
    public T? Result { get; set; }
    public string? ErrorMessage { get; set; }
}