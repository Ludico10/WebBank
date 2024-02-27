using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class AccountService
{
    protected readonly MySQLContext _context;

    public BankAccount DevelopmentFund { get; }
    public BankAccount CashAccount { get; }

    public AccountService(MySQLContext context)
    {
        _context = context;
        try
        {
            DevelopmentFund = _context.BankAccounts.Where(ba => ba.Type == AccountType.Fund).First();
            CashAccount = _context.BankAccounts.Where(ba => ba.Type == AccountType.Cash).First();
        }
        catch
        {
            throw new Exception("Bank accounts not found");
        }
    }

    public void SetStartingFund(int amount)
    {
        DevelopmentFund.Credit = amount;
    }

    private static int GetNumberPlace(int number)
    {
        int result = 1;
        while (number >= 10)
        {
            result++;
            number /= 10;
        }
        return result;
    }

    private static int CalculateChecksum(string number)
    {
        int oddSum = 0;
        int evenSum = 0;
        for (int i = 0; i < number.Length - 1; i += 2)
        {
            oddSum += number[i] - '0';
            evenSum += number[i + 1] - '0';
        }
        return 9 - (3 * oddSum + evenSum) % 10;
    }

    protected string GetFreeNumber(string prefix)
    {
        var maxID = _context.BankAccounts.Max(u => u.Id);
        var places = GetNumberPlace(maxID + 1);
        string result = prefix;
        for (int i = 0; i < 8 - places; i++)
            result += "0";
        result += (maxID + 1).ToString();
        return result + CalculateChecksum(result).ToString();
    }

    protected async Task MakeTransaction(BankAccount? fromAccount, bool fromDebet, BankAccount? toAccount, bool toDebet, DateTime time, decimal ammount)
    {
        if (fromAccount == null && toAccount == null)
        {
            return;
        }
        if (fromAccount != null && toAccount != null && fromAccount!.Currency.Id != toAccount!.Currency!.Id)
        {
            throw new Exception("Transattion in different currencies");
        }

        Transaction transaction = new()
        {
            Currency = fromAccount != null ? fromAccount.Currency : toAccount!.Currency,
            FromAccount = fromAccount,
            FromDebet = fromDebet,
            ToAccount = toAccount,
            ToDebet = toDebet,
            Time = time,
            Amount = ammount
        };
        if (fromAccount != null)
        {
            if (fromDebet)
                fromAccount.Debet += ammount;
            else
                fromAccount.Credit += ammount;
        }
        if (toAccount != null)
        {
            if (toDebet)
                toAccount.Debet += ammount;
            else
                toAccount.Credit += ammount;
        }
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
}
