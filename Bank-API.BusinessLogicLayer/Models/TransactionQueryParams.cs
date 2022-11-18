namespace Bank_API.BusinessLogicLayer.Models
{
    public class TransactionQueryParams
    {
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;
        public int? Limit { get; set; } = 10;
        public int? TransactionAmountFrom { get;set; } = null;
        public int? TransactionAmountTo { get; set; } = null;
        public int? Offset { get; set; } = 0;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
