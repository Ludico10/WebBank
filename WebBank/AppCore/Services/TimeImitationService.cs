namespace WebBank.AppCore.Services
{
    public class TimeImitationService(DateTime dateTime, DepositService depositService)
    {
        private readonly DepositService _depositService = depositService;

        private DateTime sysDate = dateTime;

        public void SkipToDate(DateTime date)
        {
            while (sysDate.CompareTo(date) < 0)
            {
                //выдача клиентам средств по завершенным программам
                _depositService.DailyCompletion(sysDate);
                //снятие клиентами набежавших по отзывным вкладам процентов
                _depositService.DailyInterestWithdrawal(sysDate);
                //начисление процентов в конце дня
                _depositService.Process(sysDate);
                //начало нового дня
                sysDate = sysDate.AddDays(1);
            }
        }
    }
}
