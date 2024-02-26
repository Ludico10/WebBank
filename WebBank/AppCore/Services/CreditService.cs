using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class CreditService(MySQLContext context) : AccountService(context), ICreditService
    {
        public async Task Create(Client client, CreditProgram program, decimal amount, DateTime sysTime, string name = "Безымянный")
        {
            if (amount < program.MinimumPayment || amount > program.MaximumPayment)
            {
                return;
            }
            BankAccount curAccount = new() { Currency = program.Currency, Name = name + "_текущий", Number = GetFreeNumber("3014"), Type = AccountType.Current };
            await _context.BankAccounts.AddAsync(curAccount);
            BankAccount percAccount = new() { Currency = program.Currency, Name = name + "_процентный", Number = GetFreeNumber("2400"), Type = AccountType.Percent };
            await _context.BankAccounts.AddAsync(percAccount);
            ClientCredit credit = new()
            {
                Client = client,
                CurrentAccount = curAccount,
                PercentAccount = percAccount,
                Program = program,
                StartDate = sysTime,
                LastPayment = sysTime,
                Amount = amount
            };
            await _context.ClientCredits.AddAsync(credit);
            await CreatePaymentSchedule(credit);
            //выделение кредита банком
            await MakeTransaction(DevelopmentFund, true, curAccount, true, sysTime, amount);
            //перевод кредита в кассу
            await MakeTransaction(curAccount, false, CashAccount, true, sysTime, amount);
            //получение кредита через кассу
            await MakeTransaction(CashAccount, false, null, false, sysTime, amount);

            await _context.SaveChangesAsync();
        }

        private async Task CreatePaymentSchedule(ClientCredit credit)
        {
            var paymentPeriod = (decimal)credit.Program.Period / credit.Program.PaymentCount;
            var creditBalance = credit.Amount;
            for (int i = 1; i <= credit.Program.PaymentCount; i++)
            {
                var dateDif = Convert.ToInt32(paymentPeriod * i);
                var date = credit.StartDate.AddDays(dateDif);
                var daysInYear = DateTime.IsLeapYear(date.Year) ? 366.0m : 365.0m;
                var m = credit.Program.Percent * paymentPeriod / daysInYear / 100.0m;
                
                decimal paymentAmount;
                decimal bodyAmount;
                decimal percentAmount;
                if (credit.Program.IsDifferentiated)
                {
                    bodyAmount = credit.Amount / credit.Program.PaymentCount;
                    paymentAmount = bodyAmount + creditBalance * m;
                    percentAmount = paymentAmount - bodyAmount;
                }
                else
                {
                    paymentAmount = (credit.Amount * m * (decimal)(Math.Pow((double)m + 1, credit.Program.Period) / (Math.Pow((double)m + 1.0, credit.Program.Period) - 1)));
                    percentAmount = creditBalance * m;
                    bodyAmount = paymentAmount - percentAmount;
                }

                CreditSchedule schedule = new()
                {
                    Credit = credit,
                    Date = new DateOnly(date.Year, date.Month, date.Day),
                    CreditBalance = creditBalance,
                    PaymentAmount = paymentAmount,
                    BodyAmount = bodyAmount,
                    PercentAmount = percentAmount,
                    PaymentTime = null
                };
                await _context.Schedules.AddAsync(schedule);

                creditBalance -= bodyAmount;
            }
        }

        public async Task Process(DateTime sysTime)
        {
            var sysDate = new DateOnly(sysTime.Year, sysTime.Month, sysTime.Day);
            foreach (var schedule in await _context.Schedules.Where(s => s.PaymentTime == null && s.Date == sysDate).ToListAsync()) 
            { 
                //начисление процентов банком
                await MakeTransaction(schedule.Credit.PercentAccount, false, DevelopmentFund, false, sysTime, schedule.PercentAmount);
            }
        }

        public async Task RepaymentInTime(DateTime sysTime)
        {
            var sysDate = new DateOnly(sysTime.Year, sysTime.Month, sysTime.Day);
            foreach (var schedule in await _context.Schedules.Where(s => s.Date == sysDate).ToListAsync())
            {
                await PeriodRepayment(schedule, sysTime);
            }
        }

        public async Task PeriodRepayment(CreditSchedule schedule, DateTime time)
        {
            if (schedule.PaymentTime != null)
            {
                return;
            }
            //внесение % в кассу
            await MakeTransaction(null, false, CashAccount, true, time, schedule.PercentAmount);
            //перевод % из кассы
            await MakeTransaction(CashAccount, false, schedule.Credit.PercentAccount, true, time, schedule.PercentAmount);
            //внесение денег в кассу
            await MakeTransaction(null, false, CashAccount, true, time, schedule.BodyAmount);
            //погашение долга за кредит из кассы
            await MakeTransaction(CashAccount, false, schedule.Credit.CurrentAccount, true, time, schedule.BodyAmount);
            //окончание кредита
            await MakeTransaction(schedule.Credit.CurrentAccount, false, DevelopmentFund, false, time, schedule.BodyAmount);

            schedule.PaymentTime = time;
            await _context.SaveChangesAsync();
        }
    }
}
