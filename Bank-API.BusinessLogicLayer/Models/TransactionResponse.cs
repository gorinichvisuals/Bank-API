namespace Bank_API.BusinessLogicLayer.Models
{
    public class TransactionResponse
    {
        public int? Id { get; set; }
        public long? Amount { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public string? Peer { get; set; }
        public long? ResultingBalance { get; set; }
        public DateTime? Date { get; set; }  
    }
}
