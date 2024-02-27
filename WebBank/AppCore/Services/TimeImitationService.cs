using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class TimeImitationService : ITimeService
    {
        private readonly IDepositService _depositService;
        private readonly ICreditService _creditService;
        private readonly MySQLContext _context;

        public DateTime sysTime;

        public TimeImitationService(MySQLContext context, IDepositService depositService, ICreditService creditService)
        {
            _creditService = creditService;
            _depositService = depositService;
            _context = context;
            var difInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "TimeDifference");
            var dif = (difInfo != null) ? Convert.ToInt32(difInfo.Value) : 0;
            sysTime = DateTime.Now.AddDays(dif);
            var lastVisitInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "LastVisitTime");
            if (lastVisitInfo != null && lastVisitInfo.Value != null)
            {
                DateTime lastVisit = DateTime.Parse(lastVisitInfo.Value);
                SkipDays(lastVisit);
            }
        }

        ~TimeImitationService()
        {
            try
            {
                var lastVisitInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "LastVisitTime");
                if (lastVisitInfo != null)
                    lastVisitInfo.Value = sysTime.ToString();
                _context.SaveChanges();
            }
            catch
            {
                // Ignore
            }
        }

        public DateOnly GetSystemDate() => DateOnly.FromDateTime(sysTime);

        public async Task SkipDays(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //внос клиентами платежей по кредитам
                _creditService.RepaymentInTime(sysTime).Wait();
                //выдача клиентам средств по завершенным программам
                _depositService.DailyCompletion(sysTime).Wait();
                //снятие клиентами набежавших по отзывным вкладам процентов
                _depositService.DailyInterestWithdrawal(sysTime).Wait();
                //начало нового дня
                sysTime = sysTime.AddDays(1);
                //снятие процентов за кредиты в 00:00
                _creditService.Process(sysTime).Wait();
                //начисление процентов на депозиты в 00:00
                _depositService.Process(sysTime.Date).Wait();
            }

            var timeInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "TimeDifference");
            if (timeInfo != null)
                timeInfo.Value = (Convert.ToInt32(timeInfo.Value) + count).ToString();
            await _context.SaveChangesAsync();
        }

        private void SkipDays(DateTime fromDate)
        {
            while (sysTime.Date.CompareTo(fromDate.Date) > 0)
            {
                //внос клиентами платежей по кредитам
                _creditService.RepaymentInTime(sysTime).Wait();
                //выдача клиентам средств по завершенным программам
                _depositService.DailyCompletion(fromDate).Wait();
                //снятие клиентами набежавших по отзывным вкладам процентов
                _depositService.DailyInterestWithdrawal(fromDate).Wait();
                //начало нового дня
                fromDate = fromDate.AddDays(1);
                //снятие процентов за кредиты в 00:00
                _creditService.Process(sysTime).Wait();
                //начисление процентов в 00:00
                _depositService.Process(fromDate.Date).Wait();
            }
        }
    }
}
