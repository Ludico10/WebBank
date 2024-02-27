namespace WebBank.AppCore.Interfaces;

public interface ITimeService
{
    public DateOnly GetSystemDate();
    public Task SkipDays(int count);
}
