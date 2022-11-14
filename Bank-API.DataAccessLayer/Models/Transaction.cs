using Bank_API.DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.DataAccessLayer.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public Card? Card { get; set; }

        [Required]
        public long? Amount { get; set; }

        [StringLength(255)]
        public string? Message { get; set; }

        [Required]
        public TransactionType? Type { get; set; }
        public string? Peer { get; set; }

        [Required]
        public long? ResultingBalance { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; }
    }
}
