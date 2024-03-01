using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;
using static System.Math;

namespace WebBank.Pages;

public class ClientProgramsModel(
    MySQLContext context,
    IDepositService depositService,
    ICreditService creditService,
    ITimeService timeService) : PageModel
{
    private const int itemsOnPage = 5;

    public DateOnly SystemDate => timeService.GetSystemDate();
    public int ClientId { get; set; }
    public string ClientName { get; set; } = "";
    public List<ClientDeposit> Deposits => depositService.GetClientPage(ClientId, DepositPage, itemsOnPage).Result;
    public int DepositPage { get; set; } = 1;
    public int DepositPagesCount { get; set; }
    public List<ClientCredit> Credits => creditService.GetClientPage(ClientId, CreditPage, itemsOnPage).Result;
    public int CreditPage { get; set; } = 1;
    public int CreditPagesCount { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, int depositPage = 1, int creditPage = 1)
    {
        ClientId = id;
        DepositPage = depositPage;
        CreditPage = creditPage;

        var depositsCount = await depositService.ClientDepositsCount(id);
        DepositPagesCount = (int)Max(Ceiling((double)depositsCount / itemsOnPage), 1);

        if (depositPage <= 0 || depositPage > DepositPagesCount)
        {
            return NotFound();
        }

        var creditsCount = await creditService.ClientCreditsCount(id);
        CreditPagesCount = (int)Max(Ceiling((double)creditsCount / itemsOnPage), 1);

        if (creditPage <= 0 || creditPage > CreditPagesCount)
        {
            return NotFound();
        }

        var client = context.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null || !client.IsActive)
        {
            return NotFound();
        }
        ClientName = client.Surname + " " + client.Name + " " + client.Patronymic;

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, int depositId, int depositPage = 1, int creditPage = 1)
    {
        var deposit = context.ClientDeposits.FirstOrDefault(cd => cd.Id == depositId);
        if (deposit == null)
        {
            return BadRequest();
        }

        var sysTime = new DateTime(SystemDate.Year, SystemDate.Month, SystemDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        await depositService.Completion(deposit, sysTime);
        return RedirectToPage(new { id, depositPage, creditPage });
    }
}
