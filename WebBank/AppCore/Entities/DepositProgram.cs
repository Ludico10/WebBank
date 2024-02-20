namespace WebBank.AppCore.Entities
{
    public class DepositProgram
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Period { get; set; }
        public int Percent {  get; set; }
        public required virtual Currency Currency { get; set; }
        public required DepositType Type { get; set; }
        public int? PercentAccessPeriod { get; set; }
        public required int MinimumPayment { get; set; } 
    }
}
