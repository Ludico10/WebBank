using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class DepositService : IDepositService
    {
        private readonly MySQLContext _context;

        public BankAccount DevelopmentFund { get; }
        public BankAccount CashAccount { get; }

        public DepositService(MySQLContext context)
        {
            _context = context;
            try
            {
                DevelopmentFund = _context.BankAccounts.Where(ba => ba.Type == AccountType.Fund).First();
                CashAccount = _context.BankAccounts.Where(ba => ba.Type == AccountType.Cash).First();
            }
            catch
            {
                throw new Exception("Bank accounts not found");
            }
        }

        public void SetStartingFund(int amount)
        {
            DevelopmentFund.Credit = amount;
        }

        public async Task<int> ClientDepositsCount(int clientId)
        {
            return await _context.ClientDeposits
                    .Where(cd => cd.Client.Id == clientId && cd.IsActive)
                    .CountAsync();
        }

        public async Task<List<ClientDeposit>> GetClientPage(int clientId, int pageNumber, int itemsOnPage)
        {
            var page = await _context.ClientDeposits
                        .OrderBy(cd => cd.StartDate)
                        .Where(cd => cd.IsActive && cd.Client.Id == clientId)
                        .Skip(itemsOnPage * (pageNumber - 1))
                        .Take(itemsOnPage)
                        .ToListAsync();

            return page;
        }

        private async Task MakeTransaction(BankAccount? fromAccount, bool fromDebet, BankAccount? toAccount, bool toDebet, DateTime time, int ammount)
        {
            if (fromAccount != null || toAccount != null)
            {
                if (fromAccount != null && toAccount != null && fromAccount.Currency.Id != toAccount.Currency.Id)
                {
                    throw new Exception("Transattion in different currencies");
                }

                Transaction transaction = new()
                {
                    Currency = fromAccount != null ? fromAccount.Currency : toAccount!.Currency,
                    FromAccount = fromAccount,
                    FromDebet = fromDebet,
                    ToAccount = toAccount,
                    ToDebet = toDebet,
                    Time = time,
                    Amount = ammount
                };
                if (fromAccount != null)
                {
                    if (fromDebet)
                        fromAccount.Debet += ammount;
                    else
                        fromAccount.Credit += ammount;
                }
                if (toAccount != null)
                {
                    if (toDebet)
                        toAccount.Debet += ammount;
                    else
                        toAccount.Credit += ammount;
                }
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
        }

        private static int GetNumberPlace(int number)
        {
            int result = 1;
            while (number >= 10)
            {
                result++;
                number /= 10;
            }
            return result;
        }

        private static int CalculateChecksum(string number)
        {
            int oddSum = 0;
            int evenSum = 0;
            for (int i = 0; i < number.Length - 1; i += 2)
            {
                oddSum += number[i] - '0';
                evenSum += number[i + 1] - '0';
            }
            return 9 - (3 * oddSum + evenSum) % 10;
        }

        private string GetFreeNumber(string prefix)
        {
            var maxID = _context.BankAccounts.Max(u => u.Id);
            var places = GetNumberPlace(maxID + 1);
            string result = prefix;
            for (int i = 0; i < 8 - places; i++)
                result += "0";
            result += (maxID + 1).ToString();
            return result + CalculateChecksum(result).ToString();
        }

        public async Task Create(Client client, DepositProgram depositProgram, int ammount, DateTime date, string name = "Безымянный")
        {
            if (ammount >= depositProgram.MinimumPayment)
            {
                BankAccount curAccount = new() { Currency = depositProgram.Currency, Name = name + "_текущий", Number = GetFreeNumber("3014"), Type = AccountType.Current };
                await _context.BankAccounts.AddAsync(curAccount);
                BankAccount percAccount = new() { Currency = depositProgram.Currency, Name = name + "_процентный", Number = GetFreeNumber("2400"), Type = AccountType.Percent };
                 await _context.BankAccounts.AddAsync(percAccount);
                var endTime = date.AddDays(depositProgram.Period);
                ClientDeposit deposit = new()
                {
                    Client = client,
                    CurrentAccount = curAccount,
                    PercentAccount = percAccount,
                    Program = depositProgram,
                    StartDate = date,
                    LastAccess = date,
                    EndDate = endTime,
                    InitialAmount = ammount
                };
                await _context.ClientDeposits.AddAsync(deposit);
                //внесение денег в кассу
                await MakeTransaction(null, false, CashAccount, true, date, ammount);
                //перевод денег с кассы на текущий счет
                await MakeTransaction(CashAccount, false, curAccount, false, date, ammount);
                //использование денег банком
                await MakeTransaction(curAccount, true, DevelopmentFund, false, date, ammount);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Process(DateTime systemDate)
        {
            foreach (var deposit in await _context.ClientDeposits.Where(d => d.IsActive).ToListAsync())
            {
                double daysCount = DateTime.IsLeapYear(systemDate.Year) ? 366.0 : 365.0;
                if (deposit.EndDate.Date.CompareTo(systemDate.Date) > 0)
                {
                    var incValue = Convert.ToInt32(deposit.InitialAmount * deposit.Program.Percent / daysCount / 100.0);
                    //начисление процентов по депозиту
                    await MakeTransaction(DevelopmentFund, true, deposit.PercentAccount, false, systemDate, incValue);
                }
            }
        }

        public async Task DailyInterestWithdrawal(DateTime sysDate)
        {
            foreach (var deposit in await _context.ClientDeposits.Where(d => d.IsActive).ToListAsync())
            {
                await InterestWithdrawal(deposit, sysDate);
            }
        }

        public async Task InterestWithdrawal(ClientDeposit deposit, DateTime time)
        {
            bool irrevocableCondition = deposit.Program.Type == DepositType.Revocable &&
                                        deposit.Program.PercentAccessPeriod != null &&
                                        deposit.LastAccess.Date.AddDays(deposit.Program.PercentAccessPeriod.Value) <= time.Date;

            if (deposit.IsActive && deposit.LastAccess.Date.CompareTo(time.Date) < 0 &&
                (deposit.EndDate.Date.CompareTo(time.Date) <= 0 || irrevocableCondition))
            {
                var percValue = deposit.PercentAccount.Credit - deposit.PercentAccount.Debet;
                //перевод процентов в кассу
                await MakeTransaction(deposit.PercentAccount, true, CashAccount, true, time, percValue);
                //вывод процентов из кассы
                await MakeTransaction(CashAccount, false, null, false, time, percValue);

                deposit.LastAccess = time.Date;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DailyCompletion(DateTime systemDate)
        {
            foreach (var deposit in await _context.ClientDeposits.Where(d => d.IsActive).ToListAsync())
            {
                await Completion(deposit, systemDate);
            }
        }

        public async Task Completion(ClientDeposit deposit, DateTime time)
        {
            if (deposit.IsActive && (deposit.Program.Type == DepositType.Revocable || deposit.EndDate.Date.CompareTo(time.Date) <= 0))
            {
                await InterestWithdrawal(deposit, time);
                //окончание депозита
                await MakeTransaction(DevelopmentFund, true, deposit.CurrentAccount, false, time, deposit.InitialAmount);
                //перевод депозита в кассу
                await MakeTransaction(deposit.CurrentAccount, true, CashAccount, true, time, deposit.InitialAmount);
                //выплата денег из кассы
                await MakeTransaction(CashAccount, false, null, false, time, deposit.InitialAmount);

                deposit.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
