using Bank_API.BusinessLogicLayer.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardTransferRequest
    {
        [Required]
        public long? Amount { get; set; }

        [Required]
        [CardNumberValidation(ErrorMessage ="Card number is invalid")]
        public long? CardNumber { get; set; }

        [StringLength(128)]
        public string? Message { get; set; }
    }
}
