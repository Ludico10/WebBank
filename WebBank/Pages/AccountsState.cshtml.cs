using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages;

public class AccountsStateModel(MySQLContext context,
    IDepositService depositService,
    ITimeService timeService) : PageModel
{
    public DateOnly SystemDate => timeService.GetSystemDate();
    public BankAccount CashAccount => depositService.CashAccount;
    public BankAccount FundAccount => depositService.DevelopmentFund;
    public List<BankAccount> Accounts => context
        .BankAccounts
        .Where(ba => ba.Type == AccountType.Percent || ba.Type == AccountType.Current)
        .ToList();
    public List<Transaction> Transactions => context
        .Transactions
        .OrderByDescending(t => t.Time)
        .ThenByDescending(t => t.Id)
        .ToList();

    public string FindOwner(BankAccount account, bool isCurrentAccount)
    {
        var deposit = context.ClientDeposits.FirstOrDefault(cd =>
                    (isCurrentAccount && cd.CurrentAccount.Id == account.Id) ||
                    (!isCurrentAccount && cd.PercentAccount.Id == account.Id));

        return deposit == null
            ? throw new Exception("—чет не прив€зан к клиенту")
            : deposit.Client.Surname + " " + deposit.Client.Name + " " + deposit.Client.Patronymic;
    }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync(int dateDif)
    {
        if (dateDif < 0)
            return RedirectToPage("Error", new { message = "Date difference can not be negative" });

        await timeService.SkipDays(dateDif);
        return Page();
    }
}
