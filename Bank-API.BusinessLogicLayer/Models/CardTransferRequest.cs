﻿using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Models
{
    public class CardTransferRequest
    {
        [Required]
        public long? Amount { get; set; }

        [Required]
        [StringLength(16)]
        public long? CardNumber { get; set; }

        [StringLength(128)]
        public string? Message { get; set; }
    }
}
