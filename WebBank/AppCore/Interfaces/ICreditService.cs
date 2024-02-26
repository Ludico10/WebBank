using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface ICreditService
{
    public Task Create(Client client, CreditProgram program, decimal amount, DateTime sysTime, string name = "Безымянный");
    public Task Process(DateTime sysTime);
    public Task RepaymentInTime(DateTime sysTime);
    public Task PeriodRepayment(CreditSchedule schedule, DateTime time);
}
