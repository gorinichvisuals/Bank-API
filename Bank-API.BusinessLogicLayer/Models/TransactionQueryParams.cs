using Bank_API.BusinessLogicLayer.Helpers;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class TransactionQueryParams
    {
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;
        public int Limit { get; set; } = 10;
        public int? TransactionAmountFrom { get;set; } = null;
        public int? TransactionAmountTo { get; set; } = null;
        public int? Offset { get; set; } = 0;

        [SortValidation(AllowableValues = new[] { "amount", "type", "created_at" }, ErrorMessage = "Value must be either 'amount' or 'type' or 'created_at'.")]
        public string? SortBy { get; set; }

        [SortValidation(AllowableValues = new[] { "DESC", "ASC" }, ErrorMessage = "Value must be either 'DESC' or 'ASC'.")]
        public string? SortDirection { get; set; }
    }
}
