using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class TimeImitationService : ITimeService
    {
        private readonly IDepositService _depositService;
        private readonly MySQLContext _context;

        public DateTime sysTime;

        public TimeImitationService(MySQLContext context, IDepositService depositService)
        {
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
            var lastVisitInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "LastVisitTime");
            if (lastVisitInfo != null)
            {
                lastVisitInfo.Value = sysTime.ToString();
            }
            _context.SaveChanges();
        }

        public DateOnly GetSystemDate()
        {
            return new DateOnly(sysTime.Year, sysTime.Month, sysTime.Day);
        }

        public async Task SkipDays(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //выдача клиентам средств по завершенным программам
                _depositService.DailyCompletion(sysTime).Wait();
                //снятие клиентами набежавших по отзывным вкладам процентов
                _depositService.DailyInterestWithdrawal(sysTime).Wait();
                //начало нового дня
                sysTime = sysTime.AddDays(1);
                //начисление процентов в 00:00
                _depositService.Process(sysTime.Date).Wait();
            }

            var timeInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "TimeDifference");
            if (timeInfo != null)
                timeInfo.Value = (Convert.ToInt32(timeInfo.Value) + count).ToString();
            await _context.SaveChangesAsync();
        }

        private void SkipDays(DateTime fromDate)
        {
            var count = (sysTime - fromDate).Days;
            while (sysTime.Date.CompareTo(fromDate.Date) > 0)
            {
                //выдача клиентам средств по завершенным программам
                _depositService.DailyCompletion(fromDate).Wait();
                //снятие клиентами набежавших по отзывным вкладам процентов
                _depositService.DailyInterestWithdrawal(fromDate).Wait();
                //начало нового дня
                fromDate = fromDate.AddDays(1);
                //начисление процентов в 00:00
                _depositService.Process(fromDate.Date).Wait();
            }

            var timeInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "TimeDifference");
            if (timeInfo != null)
                timeInfo.Value = (Convert.ToInt32(timeInfo.Value) + count).ToString();
            _context.SaveChanges();
        }
    }
}
