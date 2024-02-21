using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class AccountsStateModel(MySQLContext context, IDepositService depositService) : PageModel
    {
        private readonly IDepositService _depositService = depositService;
        private readonly MySQLContext _context = context;

        public BankAccount CashAccount { get; } = depositService.CashAccount;
        public BankAccount FundAccount { get; } = depositService.DevelopmentFund;
        public List<BankAccount> CurrentAccounts { get; set; } = [];
        public List<BankAccount> PercentAccounts { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];

        public string FindOwner(BankAccount account, bool isCurrentAccount)
        {
            var deposit = _context.ClientDeposits.First(cd => 
                        (isCurrentAccount && cd.CurrentAccount.Id == account.Id) || 
                        (!isCurrentAccount && cd.PercentAccount.Id == account.Id));

            return deposit == null
                ? throw new Exception("—чет не прив€зан к клиенту")
                : deposit.Client.Surname + " " + deposit.Client.Name + " " + deposit.Client.Patronymic;
        }

        public void OnGet()
        {
            CurrentAccounts = _context.BankAccounts.Where(ba => ba.Type == AccountType.Current).ToList();
            PercentAccounts = _context.BankAccounts.Where(ba => ba.Type == AccountType.Percent).ToList();
            Transactions = _context.Transactions.OrderBy(t => t.Time).ToList();
        }
    }
}
