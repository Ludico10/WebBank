namespace WebBank.AppCore.Entities
{
    public class ClientCredit
    {
        public int Id { get; set; }
        public required virtual Client Client { get; set; }
        public required virtual CreditProgram Program { get; set; }
        public required virtual BankAccount CurrentAccount { get; set; }
        public required virtual BankAccount PercentAccount { get; set; }
        public int Amount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime LastPayment { get; set; }
    }
}
