﻿using System.ComponentModel.DataAnnotations;

namespace Bank_API.BusinessLogicLayer.Helpers
{
    public class CardNumberValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string? toStrValue = value?.ToString();
            string? str = toStrValue!.Substring(0, toStrValue.Length - 4);

            return str == "414156236523" && toStrValue.Length == 16;
        }
    }
}