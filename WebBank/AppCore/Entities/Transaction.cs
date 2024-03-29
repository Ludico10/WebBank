﻿namespace WebBank.AppCore.Entities;

public class Transaction
{
    public int Id { get; set; }
    public virtual required Currency Currency { get; set; }
    public required virtual BankAccount? FromAccount { get; set; }
    public bool FromDebet { get; set; } = false;
    public required virtual BankAccount? ToAccount { get; set; }
    public bool ToDebet { get; set; } = false;
    public DateTime Time { get; set; }
    public decimal Amount { get; set; }
}
