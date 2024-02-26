using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities
{
    public class CreditProgram
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Percent { get; set; }
        public int Period { get; set; }

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public bool IsDifferentiated { get; set; }
        public int PaymentCount { get; set; }
        public int MinimumPayment { get; set; }
        public int MaximumPayment { get; set; }
    }
}
