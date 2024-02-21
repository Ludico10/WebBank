using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces
{
    public interface IDepositService
    {
        public BankAccount DevelopmentFund { get; }
        public BankAccount CashAccount { get; }

        public void SetStartingFund(int amount);
        public void Create(Client client, DepositProgram depositProgram, int ammount, DateTime date, string name = "Безымянный");
        public void Process(DateTime systemDate);
        public void DailyInterestWithdrawal(DateTime sysDate);
        public void InterestWithdrawal(ClientDeposit deposit, DateTime time);
        public void DailyCompletion(DateTime systemDate);
        public void Completion(ClientDeposit deposit, DateTime time);
    }
}
