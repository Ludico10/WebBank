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
            var timeInfo = _context.SystemInformation.FirstOrDefault(si => si.Name == "TimeDifference");
            var dif = (timeInfo != null) ? Convert.ToInt32(timeInfo.Value) : 0;
            sysTime = DateTime.Now.AddDays(dif);
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
    }
}
