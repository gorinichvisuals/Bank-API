using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_API.DataAccessLayer.Models
{
    public enum Currency
    {
        UAH,
        USD,
        EUR
    }

    public enum CardStatus
    {
        Active,
        Closed,
        Frozen
    }

    public class Card
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public User? User { get; set; }

        [Required]
        public DateTime? Exp { get; set; }

        [Required]
        [StringLength(3)]
        public short? Cvv { get; set; }

        [Required]
        [StringLength(16)]
        public long? Number { get; set; }

        [Required]
        public Currency? Currency { get; set; }

        [Required]
        public long? Balance { get; set; }

        [Required]
        public CardStatus? Status { get; set; }

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
