using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class AccountsStateModel(MySQLContext context,
        IDepositService depositService,
        ITimeService timeService) : PageModel
    {
        public DateOnly SystemDate => timeService.GetSystemDate();
        public BankAccount CashAccount { get; } = depositService.CashAccount;
        public BankAccount FundAccount { get; } = depositService.DevelopmentFund;
        public List<BankAccount> CurrentAccounts { get; set; } = [];
        public List<BankAccount> PercentAccounts { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];

        public string FindOwner(BankAccount account, bool isCurrentAccount)
        {
            var deposit = context.ClientDeposits.First(cd =>
                        (isCurrentAccount && cd.CurrentAccount.Id == account.Id) ||
                        (!isCurrentAccount && cd.PercentAccount.Id == account.Id));

            return deposit == null
                ? throw new Exception("—чет не прив€зан к клиенту")
                : deposit.Client.Surname + " " + deposit.Client.Name + " " + deposit.Client.Patronymic;
        }

        public void OnGetAsync()
        {
            CurrentAccounts = context.BankAccounts.Where(ba => ba.Type == AccountType.Current).ToList();
            PercentAccounts = context.BankAccounts.Where(ba => ba.Type == AccountType.Percent).ToList();
            Transactions = context.Transactions.OrderBy(t => t.Time).ToList();
        }

        public async Task<IActionResult> OnPostAsync(int dateDif)
        {
            if (dateDif < 0)
                return RedirectToPage("Error");

            await timeService.SkipDays(dateDif);
            return Page();
        }
    }
}
