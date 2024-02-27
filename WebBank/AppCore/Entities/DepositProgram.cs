using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

public class DepositProgram
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Period { get; set; }
    public int Percent { get; set; }

    [ForeignKey(nameof(Currency))]
    public int CurrencyId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual Currency Currency { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public bool IsRevocable { get; set; }
    public int? PercentAccessPeriod { get; set; }
    public decimal MinimumPayment { get; set; }
}
