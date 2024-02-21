using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public required AccountType Type { get; set; }
        public int Debet { get; set; } = 0;
        public int Credit { get; set; } = 0;
    }
}
