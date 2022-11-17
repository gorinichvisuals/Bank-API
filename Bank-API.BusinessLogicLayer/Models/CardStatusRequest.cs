using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardStatusRequest
    {
        [Required]
        public bool? FreezeCard { get; set; }
    }
}
