using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardStatusRequest
    {
        public bool? FreezeCard { get; set; }
    }
}
