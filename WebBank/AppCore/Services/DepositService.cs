using Google.Protobuf.WellKnownTypes;
using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class DepositService
    {
        private readonly MySQLContext _context;

        private BankAccount DevelopmentFund { get; }
        private BankAccount CashAccount { get; }

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

        private void MakeTransaction(BankAccount? fromAccount, bool fromDebet, BankAccount? toAccount, bool toDebet, DateTime time, int ammount)
        {
            Transaction transaction = new()
            {
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
            _context.SaveChanges();
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

        public void Create(Client client, DepositProgram depositProgram, int ammount, DateTime date, string name = "Безымянный")
        {
            if (ammount >= depositProgram.MinimumPayment)
            {
                BankAccount curAccount = new() { Currency = depositProgram.Currency, Name = name + "_текущий", Number = GetFreeNumber("3014"), Type = AccountType.Client };
                _context.BankAccounts.Add(curAccount);
                BankAccount percAccount = new() { Currency = depositProgram.Currency, Name = name + "_процентный", Number = GetFreeNumber("2400"), Type = AccountType.Client };
                _context.BankAccounts.Add(percAccount);
                var endTime = date.AddDays(depositProgram.Period);
                ClientDeposit deposit = new()
                {
                    Client = client,
                    CurrentAccount = curAccount,
                    PercentAccount = percAccount,
                    Program = depositProgram,
                    StartDate = date,
                    EndDate = endTime,
                    InitialAmount = ammount
                };
                _context.ClientDeposits.Add(deposit);
                //внесение денег в кассу
                MakeTransaction(null, false, CashAccount, true, date, ammount);
                //перевод денег с кассы на текущий счет
                MakeTransaction(CashAccount, false, curAccount, false, date, ammount);
                //использование денег банком
                MakeTransaction(curAccount, true, DevelopmentFund, false, date, ammount);
            }
        }

        public void Process(DateTime systemDate)
        {
            foreach (var deposit in _context.ClientDeposits.Where(d => d.IsActive))
            {
                double daysCount = DateTime.IsLeapYear(systemDate.Year) ? 366.0 : 365.0;
                if (deposit.EndDate.Date.CompareTo(systemDate.Date) > 0)
                {
                    var incValue = Convert.ToInt32(deposit.InitialAmount * deposit.Program.Percent / daysCount / 100.0);
                    //начисление процентов по депозиту
                    MakeTransaction(DevelopmentFund, true, deposit.PercentAccount, false, systemDate, incValue);
                }
            }
        }

        public void DailyInterestWithdrawal(DateTime sysDate)
        {
            foreach (var deposit in _context.ClientDeposits.Where(d => d.IsActive))
            {
                InterestWithdrawal(deposit, sysDate);
            }
        }

        public void InterestWithdrawal(ClientDeposit deposit, DateTime time)
        {
            bool irrevocableCondition = deposit.Program.Type == DepositType.Revocable &&
                                        deposit.Program.PercentAccessPeriod != null &&
                                        deposit.LastAccess.Date.AddDays(deposit.Program.PercentAccessPeriod.Value) <= time.Date;

            if (deposit.IsActive && deposit.LastAccess.Date.CompareTo(time.Date) < 0 && 
                (deposit.EndDate.Date.CompareTo(time.Date) <= 0 || irrevocableCondition))
            {
                var percValue = deposit.PercentAccount.Credit - deposit.PercentAccount.Debet;
                //перевод процентов в кассу
                MakeTransaction(deposit.PercentAccount, true, CashAccount, true, time, percValue);
                //вывод процентов из кассы
                MakeTransaction(CashAccount, false, null, false, time, percValue);

                deposit.LastAccess = time.Date;
                _context.SaveChanges();
            }
        }

        public void DailyCompletion(DateTime systemDate)
        {
            foreach (var deposit in _context.ClientDeposits.Where(d => d.IsActive))
            {
                Completion(deposit, systemDate);
            }
        }

        public void Completion(ClientDeposit deposit, DateTime time)
        {
            if (deposit.IsActive && (deposit.Program.Type == DepositType.Revocable || deposit.EndDate.Date.CompareTo(time.Date) <= 0))
            {
                InterestWithdrawal(deposit, time);
                //окончание депозита
                MakeTransaction(DevelopmentFund, true, deposit.CurrentAccount, false, time, deposit.InitialAmount);
                //перевод депозита в кассу
                MakeTransaction(deposit.CurrentAccount, true, CashAccount, true, time, deposit.InitialAmount);
                //выплата денег из кассы
                MakeTransaction(CashAccount, false, null, false, time, deposit.InitialAmount);

                deposit.IsActive = false;
                _context.SaveChanges();
            }
        }
    }
}
