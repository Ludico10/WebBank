using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

public class CreditSchedule
{
    public int Id { get; set; }
    [ForeignKey(nameof(Credit))]
    public int CreditId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual ClientCredit Credit { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DateOnly Date { get; set; }
    public decimal CreditBalance { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal BodyAmount { get; set; }
    public decimal PercentAmount { get; set; }
    public DateTime? PaymentTime { get; set; }
}
