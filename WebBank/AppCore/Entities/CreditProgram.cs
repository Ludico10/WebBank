﻿using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

public class CreditProgram
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Percent { get; set; }
    public int Period { get; set; }

    [ForeignKey(nameof(Currency))]
    public int CurrencyId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual Currency Currency { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public bool IsDifferentiated { get; set; }
    public int PaymentCount { get; set; }
    public decimal MinimumPayment { get; set; }
    public decimal MaximumPayment { get; set; }
}
