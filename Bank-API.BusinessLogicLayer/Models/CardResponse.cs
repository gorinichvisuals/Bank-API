using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardResponse
    {
        public int? Id { get; set; }
        public long? Number { get; set; }
        public string? Exp { get; set; }
        public int? Cvv { get; set; }
        public string? Status { get; set; }
        public string? Currency { get; set; }
        public long? Balance { get; set; }
    }
}
