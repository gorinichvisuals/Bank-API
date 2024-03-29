﻿namespace Bank_API.BusinessLogicLayer.Models.Responses;

public class TransactionDTO
{
    public long Amount { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
    public string? Peer { get; set; }
    public long ResultingBalance { get; set; }
    public DateTime Date { get; set; }
}