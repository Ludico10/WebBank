namespace WebBank.AppCore.Entities
{
    public class CreditSchedule
    {
        public int Id { get; set; }
        public virtual ClientCredit Credit { get; set; }
        public DateOnly Date { get; set; }
        public int CreditBalance { get; set; }
        public int PaymentAmount { get; set; }
        public int BodyAmount { get; set; }
        public int PercentAmount { get; set; }
        public DateTime? PaymentTime { get; set; }
    }
}
