using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class DepositService : AccountService, IDepositService
    {
        public DepositService(MySQLContext context) : base(context) { }

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

        public async Task Create(Client client, DepositProgram depositProgram, int amount, DateTime date, string name = "Безымянный")
        {
            if (amount >= depositProgram.MinimumPayment)
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
                    InitialAmount = amount
                };
                await _context.ClientDeposits.AddAsync(deposit);
                //внесение денег в кассу
                await MakeTransaction(null, false, CashAccount, true, date, amount);
                //перевод денег с кассы на текущий счет
                await MakeTransaction(CashAccount, false, curAccount, false, date, amount);
                //использование денег банком
                await MakeTransaction(curAccount, true, DevelopmentFund, false, date, amount);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Process(DateTime systemDate)
        {
            double daysCount = DateTime.IsLeapYear(systemDate.Year) ? 366.0 : 365.0;
            foreach (var deposit in await _context.ClientDeposits.Where(d => d.IsActive).ToListAsync())
            {
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
