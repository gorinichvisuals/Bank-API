using Bank_API.BusinessLogicLayer.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardCreateRequest
    {
        [Required]
        [CurencyValAttribute]
        public string? Currency { get; set; }
    }
}
