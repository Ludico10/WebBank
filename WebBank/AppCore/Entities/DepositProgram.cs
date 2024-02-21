using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities
{
    public class DepositProgram
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Period { get; set; }
        public int Percent { get; set; }

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public required DepositType Type { get; set; }
        public int? PercentAccessPeriod { get; set; }
        public int MinimumPayment { get; set; } 
    }
}
