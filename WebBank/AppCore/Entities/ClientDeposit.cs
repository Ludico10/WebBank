namespace WebBank.AppCore.Entities;

public class ClientDeposit
{
    public int Id { get; set; }
    public required virtual Client Client { get; set; }
    public required virtual DepositProgram Program { get; set; }
    public required virtual BankAccount CurrentAccount { get; set; }
    public required virtual BankAccount PercentAccount { get; set; }
    public decimal InitialAmount { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastAccess { get; set; }
}
