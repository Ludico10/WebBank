using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces
{
    public interface IDepositService
    {
        public BankAccount DevelopmentFund { get; }
        public BankAccount CashAccount { get; }

        public void SetStartingFund(int amount);
        public Task<int> ClientDepositsCount(int clientId);
        public Task<List<ClientDeposit>> GetClientPage(int clientId, int pageNumber, int itemsOnPage);
        public Task Create(Client client, DepositProgram depositProgram, int ammount, DateTime date, string name = "Безымянный");
        public Task Process(DateTime systemDate);
        public Task DailyInterestWithdrawal(DateTime sysDate);
        public Task InterestWithdrawal(ClientDeposit deposit, DateTime time);
        public Task DailyCompletion(DateTime systemDate);
        public Task Completion(ClientDeposit deposit, DateTime time);
    }
}
