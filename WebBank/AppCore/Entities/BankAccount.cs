using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

public class BankAccount
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Number { get; set; }

    [ForeignKey(nameof(Currency))]
    public int CurrencyId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual Currency Currency { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public required AccountType Type { get; set; }
    public decimal Debet { get; set; } = 0;
    public decimal Credit { get; set; } = 0;
}
