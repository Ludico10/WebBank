namespace WebBank.AppCore.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
        public required virtual Currency Currency { get; set; }
        public required AccountType Type { get; set; }
        public int Debet { get; set; } = 0;
        public int Credit { get; set; } = 0;
    }
}
