namespace Bank_API.BusinessLogicLayer.Models
{
    public class Response<T>
    {
        public T? Result { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
